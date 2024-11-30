namespace backend.Dtos
{
    public class ClientRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string ContactInformation { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Notes { get; set; }
    }
}