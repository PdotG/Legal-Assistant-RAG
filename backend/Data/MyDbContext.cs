using backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace backend.Data
{
    public class MyDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MyDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Configuración de la conexión a PostgreSQL
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        // DbSet para las demás entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Embedding> Embeddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de las tablas
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Models.File>().ToTable("files");
            modelBuilder.Entity<Embedding>().ToTable("embeddings");

            // Ignorar la propiedad Vector para que no sea mapeada automáticamente
            modelBuilder.Entity<Embedding>().Ignore(e => e.Vector);
        }

        // Método para agregar un Embedding con SQL crudo
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

            await Database.ExecuteSqlRawAsync(sql, parameters);
        }

        // Método para recuperar Embeddings por id de archivo
        public async Task<List<Embedding>> GetEmbeddingsByFileIdAsync(int fileId)
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

            await using var connection = (NpgsqlConnection)Database.GetDbConnection();
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
    }
}
