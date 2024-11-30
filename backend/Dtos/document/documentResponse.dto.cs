namespace backend.Dtos
{
    public class DocumentResponseDto
    {
        public int IdDocument { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UploadedDate { get; set; }

        public CaseSummaryDto? Case { get; set; }

        public FileSummaryDto? File { get; set; }
    }
}