namespace backend.Models
{
    public class Client
    {
        public int IdClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInformation { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public ICollection<Case>? Cases { get; set; }
    }
}