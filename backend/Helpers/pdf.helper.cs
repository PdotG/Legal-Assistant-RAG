using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig;
using OpenAI;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using OpenAI.Embeddings;

namespace backend.Helpers
{
    public class PdfHelper
    {
        private readonly OpenAIClient _openAiClient;
        private const int BatchSize = 100;
        private const int ChunkSize = 1000; // Characters per chunk

        public PdfHelper(OpenAIClient openAiClient)
        {
            _openAiClient = openAiClient;
        }

        public async Task ProcessPdfAsync(string filePath, int fileId)
        {
            List<string> chunks = new List<string>();
            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    string pageText = page.Text;
                    List<string> pageChunks = ChunkText(pageText, ChunkSize);
                    chunks.AddRange(pageChunks);
                    if (chunks.Count >= BatchSize)
                    {
                        await ProcessBatch(chunks, fileId);
                        chunks.Clear();
                    }
                }
                if (chunks.Count > 0)
                {
                    await ProcessBatch(chunks, fileId);
                }
            }
        }

        private List<string> ChunkText(string text, int chunkSize)
        {
            List<string> chunks = new List<string>();
            for (int i = 0; i < text.Length; i += chunkSize)
            {
                chunks.Add(text.Substring(i, Math.Min(chunkSize, text.Length - i)));
            }
            return chunks;
        }

        private async Task ProcessBatch(List<string> chunks, int fileId)
        {
            EmbeddingClient client = new("text-embedding-3-small", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            List<Embedding> embeddingsToSave = new List<Embedding>();

            foreach (var chunk in chunks)
            {
                OpenAIEmbedding embedding = client.GenerateEmbedding(chunk);

                // https://www.nuget.org/packages/OpenAI#how-to-generate-text-embeddings docs
                
                ReadOnlyMemory<float> vector = embedding.ToFloats();

                Embedding embeddingToSave = new Embedding
                {
                    FileId = fileId,
                    Vector = vector.ToArray(),
                    PlainText = chunk,
                    CreatedAt = DateTime.UtcNow
                };

                embeddingsToSave.Add(embeddingToSave);
            }

            // Save embeddingsToSave to your storage system
            //await SaveEmbeddingsAsync(embeddingsToSave);
        }

    }
}