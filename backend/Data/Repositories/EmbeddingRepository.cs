using System.Text.Json;
using backend.Dtos;
using backend.Helpers;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using OpenAI;
using OpenAI.Embeddings;

namespace backend.Data.Repositories
{
    public class EmbeddingRepository : BaseRepository<Embedding>, IEmbeddingRepository
    {
        private readonly EmbeddingsHelper _embeddingsHelper;

        public EmbeddingRepository(MyDbContext context, EmbeddingsHelper embeddingsHelper) : base(context)
        {
            _embeddingsHelper = embeddingsHelper;
        }

        public async Task AddEmbeddingAsync(Embedding embedding)
        {
            var sql = @"
                INSERT INTO embeddings (id_file, embedding, chunk_index, plain_text, embedding_created_at)
                VALUES (@id_file, @embedding, @chunk_index, @plain_text, @created_at)";

            var parameters = new[]
            {
                new NpgsqlParameter("@id_file", embedding.FileId),
                new NpgsqlParameter("@embedding", embedding.Vector),
                new NpgsqlParameter("@chunk_index", embedding.ChunkIndex ?? (object)DBNull.Value),
                new NpgsqlParameter("@plain_text", embedding.PlainText ?? (object)DBNull.Value),
                new NpgsqlParameter("@created_at", embedding.CreatedAt)
            };

            await using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            await command.ExecuteNonQueryAsync();
        }

        // Método para recuperar Embeddings por id de archivo
        public async Task<IEnumerable<Embedding>> GetEmbeddingsByFileIdAsync(int fileId)
        {
            var sql = @"
                SELECT id_embedding, id_file, embedding, chunk_index, plain_text, embedding_created_at
                FROM embeddings
                WHERE id_file = @fileId";

            var parameters = new[]
            {
                new NpgsqlParameter("@fileId", fileId)
            };

            var embeddings = new List<Embedding>();

            await using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                embeddings.Add(new Embedding
                {
                    Id = reader.GetInt32(0),
                    FileId = reader.GetInt32(1),
                    Vector = reader[2] as float[] ?? Array.Empty<float>(), // Vector como float[]
                    ChunkIndex = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    PlainText = reader.IsDBNull(4) ? null : reader.GetString(4),
                    CreatedAt = reader.GetDateTime(5)
                });
            }

            return embeddings;
        }

        public async Task SaveEmbeddingsAsync(List<Embedding> embeddings)
        {
            _context.Embeddings.AddRange(embeddings);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmbeddingChunk>> SearchChunksAsync(string inputMessage, int fileId, int n = 1)
        {
            var vectorizedContent = await _embeddingsHelper.GenerateEmbeddingsFromStringAsync(inputMessage);
            var embeddingVector = vectorizedContent.SelectMany(innerArray => innerArray).ToArray();


            // Consulta a la base de datos para encontrar los embeddings más cercanos
            var sql = @"
                SELECT 
                    embeddings.plain_text,
                    1 - (embeddings.embedding <=> (@embedding::vector)) AS cosine_similarity
                FROM embeddings
                WHERE id_file = @fileId
                ORDER BY cosine_similarity DESC
                LIMIT @limit";

            var parameters = new[]
            {
                new NpgsqlParameter("@embedding", NpgsqlDbType.Array | NpgsqlDbType.Real) { Value = embeddingVector },
                new NpgsqlParameter("@fileId", fileId),
                new NpgsqlParameter("@limit", n)
        };

            var results = new List<EmbeddingChunk>();

            await using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new EmbeddingChunk
                {
                    PlainText = reader.GetString(0),
                    CosineSimilarity = reader.GetFloat(1)
                });
            }

            return results;
        }

    }


}

