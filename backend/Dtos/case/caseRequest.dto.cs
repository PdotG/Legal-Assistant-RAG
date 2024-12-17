namespace backend.Dtos
{
    public class CaseRequestDto
    {
        public required string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime? CourtDate { get; set; }
        public required int ClientId { get; set; }
        public required int AssignedUserId { get; set; }
    }
}