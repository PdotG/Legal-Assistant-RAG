using System.ClientModel;
using backend.Dtos;
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

        public ChatController(IConfiguration configuration)
        {
            string? apiKey = configuration.GetSection("OpenAI:OPENAI_API_KEY").Value;
            _chatClient = new ChatClient(model: "gpt-4o-mini", apiKey: apiKey);
        }

        [HttpPost("stream")]
        public async Task StreamChatResponse([FromBody] ChatRequestDto request)
        {
            Response.ContentType = "text/event-stream";

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("data: Error: La pregunta no puede estar vacía.\n\n");
                await Response.Body.FlushAsync();
                return;
            }

            try
            {
                AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates =
                    _chatClient.CompleteChatStreamingAsync(request.Question);

                await foreach (var update in completionUpdates)
                {
                    if (update.ContentUpdate.Count > 0)
                    {
                        string chunk = update.ContentUpdate[0].Text;
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