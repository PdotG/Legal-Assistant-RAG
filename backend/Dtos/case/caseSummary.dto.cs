namespace backend.Dtos
{
    public class CaseSummaryDto
    {
        public int IdCase { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
    }
}