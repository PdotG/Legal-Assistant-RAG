namespace backend.Dtos
{
    public class DocumentSummaryDto
    {
        public int IdDocument { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
    }
}