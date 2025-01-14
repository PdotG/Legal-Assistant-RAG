using System.ClientModel;
using backend.Data.Repositories;
using backend.Dtos;
using backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ChatClient _chatClient;
        private readonly EmbeddingsHelper _embeddingsHelper;
        private readonly EmbeddingRepository _repository;

        public ChatController(IConfiguration configuration, EmbeddingsHelper embeddingsHelper, EmbeddingRepository repository)
        {
            string? apiKey = configuration.GetSection("OpenAI:OPENAI_API_KEY").Value;
            _chatClient = new ChatClient(model: "gpt-4o-mini", apiKey: apiKey);
            _embeddingsHelper = embeddingsHelper;
            _repository = repository;
        }

        [HttpPost("ask")]
        public async Task StreamChatResponse([FromBody] ChatRequestDto request)
        {
            Response.ContentType = "text/event-stream";

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("data: Error: La pregunta no puede estar vacía.\n\n");
                await Response.Body.FlushAsync();
                return;
            }

            try
            {
                var sortedChunks = await _repository.SearchChunksAsync(request.Message, request.FileId);

                if (sortedChunks.Count == 0)
                {
                    Response.StatusCode = 404;
                    await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("No se encontraron resultados relevantes.\n"));
                    return;
                }
                var bestMatch = sortedChunks[0];
                string? answer = bestMatch.CosineSimilarity > 0.25
                    ? System.Text.RegularExpressions.Regex.Replace(
                        bestMatch.PlainText.Replace("\n", " ").Replace("\t", " ").Replace("¶", "").Trim(),
                        @"\s+(?=\d)", "")
                    : null;
                string prompt = string.IsNullOrEmpty(answer)
                    ? $"Actúa como un asistente legal llamado Legal RAG Assistant. La pregunta es: {request.Message}. No se ha encontrado una respuesta adecuada en la información almacenada."
                    : $"Actúa como un asistente legal llamado Legal RAG Assistant. La pregunta es: {request.Message}. La respuesta: {answer}. Responde a la pregunta con la respuesta que se te ha proporcionado.";

                AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates =
                    _chatClient.CompleteChatStreamingAsync(prompt);

                await foreach (var update in completionUpdates)
                {
                    if (update.ContentUpdate.Count > 0)
                    {
                        string chunk = update.ContentUpdate[0].Text;
                        chunk = System.Text.RegularExpressions.Regex.Replace(chunk, @"([a-zA-Z])(\d)", "$1 $2");
                        chunk = System.Text.RegularExpressions.Regex.Replace(chunk, @"(\d)([a-zA-Z])", "$1 $2");
                        
                        await Response.WriteAsync($"data: {chunk}\n\n");
                        await Response.Body.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync($"data: Error: {ex.Message}\n\n");
                await Response.Body.FlushAsync();
            }
        }

    }
}