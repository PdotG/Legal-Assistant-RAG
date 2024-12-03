namespace backend.Dtos
{
    public class ChatRequestDto
    {
        public required string Message { get; set; }
        public required int FileId { get; set; }

    }

}