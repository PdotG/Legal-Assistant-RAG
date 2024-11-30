namespace backend.Models
{
    public class Document
    {
        public int IdDocument { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        public int CaseId { get; set; }
        public Case? Case { get; set; }

        public int FileId { get; set; }
        public File? File { get; set; }
    }

}