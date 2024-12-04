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
        public async Task AskGPT([FromBody] ChatRequestDto request)
        {
            try
            {
                // Buscar los chunks relevantes en la base de datos
                var sortedChunks = await _repository.SearchChunksAsync(request.Message, request.FileId);

                if (sortedChunks.Count == 0)
                {
                    Response.StatusCode = 404; // Establecer código 404
                    await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("No se encontraron resultados relevantes.\n"));
                    return; // Finalizar el método
                }

                var bestMatch = sortedChunks[0];
                string? answer = bestMatch.CosineSimilarity > 0.25
                    ? bestMatch.PlainText.Replace("\n", " ").Replace("\t", " ").Replace("¶", "").Trim()
                    : null;

                string prompt = string.IsNullOrEmpty(answer)
                    ? $"Actúa como un asistente legal llamado RAG Assistant. La pregunta es: {request.Message}. No se ha encontrado una respuesta adecuada en la información almacenada."
                    : $"Actúa como un asistente legal llamado RAG Assistant. La pregunta es: {request.Message}. La respuesta: {answer}. Responde a la pregunta con la respuesta que se te ha proporcionado.";

                // Configura el tipo de contenido para el streaming
                Response.ContentType = "text/event-stream";

                // Obtener las respuestas de manera incremental usando streaming
                var chatResponseStream = _chatClient.CompleteChatStreaming(prompt);

                foreach (var update in chatResponseStream)
                {
                    if (update.ContentUpdate.Count > 0)
                    {
                        var content = update.ContentUpdate[0].Text;
                        await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(content));
                        await Response.Body.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores durante el flujo
                if (!Response.HasStarted)
                {
                    Response.StatusCode = 500;
                    await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes($"Error: {ex.Message}\n"));
                }
                else
                {
                    Console.Error.WriteLine($"Error durante la transmisión: {ex.Message}");
                }
            }
        }

    }
}