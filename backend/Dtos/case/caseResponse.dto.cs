namespace backend.Dtos
{
    public class CaseResponseDto
    {
        public int IdCase { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? CourtDate { get; set; }

        public ClientSummaryDto? Client { get; set; }

        public UserSummaryDto? AssignedUser { get; set; }

        public ICollection<DocumentSummaryDto>? Documents { get; set; }
    }
}