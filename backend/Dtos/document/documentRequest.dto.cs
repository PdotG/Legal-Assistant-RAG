namespace backend.Dtos
{
    public class DocumentRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CaseId { get; set; }
        public int FileId { get; set; }
    }
}