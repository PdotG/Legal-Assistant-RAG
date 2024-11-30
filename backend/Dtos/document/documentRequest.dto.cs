namespace backend.Dtos
{
    public class DocumentRequestDto
    {
        public required string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public required int CaseId { get; set; }
        public required int FileId { get; set; }
    }
}