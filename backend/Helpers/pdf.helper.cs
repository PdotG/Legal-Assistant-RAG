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
        private const int ChunkSize = 1000; // Tamaño máximo por chunk

        public PdfHelper(OpenAIClient openAiClient, EmbeddingRepository repository, IConfiguration configuration)
        {
            _openAiClient = openAiClient;
            _repository = repository;
            var apiKey = configuration.GetSection("OpenAI:OPENAI_API_KEY").Value;
            _embeddingClient = new EmbeddingClient("text-embedding-3-small", apiKey);
        }

        public async Task ProcessPdfAsync(string filePath, int fileId)
        {
            List<string> chunks = new List<string>();
            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    string pageText = page.Text;
                    if (pageText.Length <= ChunkSize)
                    {
                        chunks.Add(pageText);
                    }
                    else
                    {
                        List<string> pageChunks = ChunkText(pageText, ChunkSize);
                        chunks.AddRange(pageChunks);
                    }
                }
            }

            // Process chunks in batches
            for (int i = 0; i < chunks.Count; i += BatchSize)
            {
                var batchChunks = chunks.Skip(i).Take(BatchSize).ToList();
                await ProcessBatch(batchChunks, fileId);
            }
        }

        private List<string> ChunkText(string text, int chunkSize)
        {
            List<string> chunks = new List<string>();
            int offset = 0;
            while (offset < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - offset);
                chunks.Add(text.Substring(offset, length));
                offset += length;
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
                            await Task.Delay(1000);
                        }
                    }
                }
            }

            if (embeddingsToSave.Count > 0)
            {
                foreach(var embedding in embeddingsToSave){
                    await _repository.AddEmbeddingAsync(embedding);
                }
            }
        }

        public string SaveFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine("Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
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