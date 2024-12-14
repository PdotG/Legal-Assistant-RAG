namespace backend.Dtos
{
    public class ClientResponseDto
    {
        public int IdClient { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInformation { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Notes { get; set; }

        public ICollection<CaseSummaryDto>? Cases { get; set; }
    }
}