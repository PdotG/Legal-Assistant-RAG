namespace backend.Dtos
{
    public class FileSummaryDto
    {
        public int IdFile { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}