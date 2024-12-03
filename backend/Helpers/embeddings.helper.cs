
using backend.Data.Repositories;
using OpenAI;
using OpenAI.Embeddings;

namespace backend.Helpers
{
    public class EmbeddingsHelper
    {
        private readonly OpenAIClient _openAiClient;
        private readonly EmbeddingClient _embeddingClient;

        public EmbeddingsHelper(OpenAIClient openAiClient, IConfiguration configuration)
        {
            _openAiClient = openAiClient;
            var apiKey = configuration.GetSection("OpenAI:OPENAI_API_KEY").Value;
            _embeddingClient = new EmbeddingClient("text-embedding-3-small", apiKey);
        }

        public async Task<List<float[]>> GenerateEmbeddingsFromStringAsync(string text)
        {
            var embeddings = new List<float[]>();
            try
            {
                OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(text);
                float[] vector = embedding.ToFloats().ToArray();

                embeddings.Add(vector);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating embeddings: {ex.Message}");
            }

            return embeddings;
        }
    }
}