namespace backend.Dtos
{
    public class ClientRequestDto
    {
        public required string Name { get; set; } = string.Empty;
        public required string ContactInformation { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Notes { get; set; }
    }
}