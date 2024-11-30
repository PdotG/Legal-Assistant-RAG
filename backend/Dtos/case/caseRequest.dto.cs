namespace backend.Dtos
{
    public class CaseRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime? CourtDate { get; set; }
        public int ClientId { get; set; }
        public int? AssignedUserId { get; set; }
    }
}