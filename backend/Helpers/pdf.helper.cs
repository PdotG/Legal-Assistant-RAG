using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig;
using OpenAI;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using OpenAI.Embeddings;
using backend.Data.Repositories;

namespace backend.Helpers
{
    public class PdfHelper
    {
        private readonly OpenAIClient _openAiClient;
        private readonly EmbeddingRepository _repository;
        private readonly EmbeddingClient _embeddingClient;
        private const int BatchSize = 100;
        private const int ChunkSize = 1000; // Characters per chunk

        public PdfHelper(OpenAIClient openAiClient, EmbeddingRepository repository)
        {
            _openAiClient = openAiClient;
            _repository = repository;
            _embeddingClient = new EmbeddingClient("text-embedding-3-small", "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");
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
                    Console.Write(pageChunks);
                    chunks.AddRange(pageChunks);
                    if (chunks.Count >= BatchSize)
                    {
                        //await ProcessBatch(chunks, fileId);
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
            List<Embedding> embeddingsToSave = new List<Embedding>();
            int maxRetries = 3;

            for (int index = 0; index < chunks.Count; index++)
            {
                var chunk = chunks[index];
                int retryCount = 0;
                bool success = false;

                while (!success && retryCount < maxRetries)
                {
                    try
                    {
                        OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(chunk);
                        ReadOnlyMemory<float> vector = embedding.ToFloats();

                        Embedding embeddingToSave = new Embedding
                        {
                            FileId = fileId,
                            ChunkIndex = index,
                            Vector = vector.ToArray(),
                            PlainText = chunk,
                            CreatedAt = DateTime.UtcNow
                        };

                        embeddingsToSave.Add(embeddingToSave);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        if (retryCount >= maxRetries)
                        {
                            Console.WriteLine($"Error generating embedding for chunk {index} after {maxRetries} attempts: {ex.Message}");
                        }
                        else
                        {
                            Console.WriteLine($"Retrying chunk {index}, attempt {retryCount + 1}/{maxRetries}");
                            await Task.Delay(1000); // Esperar un segundo antes de reintentar
                        }
                    }
                }
            }

            if (embeddingsToSave.Count > 0)
            {
                await _repository.SaveEmbeddingsAsync(embeddingsToSave);
            }
        }

        public string SaveFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }
    }
}