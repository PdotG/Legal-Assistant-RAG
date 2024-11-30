namespace backend.Models
{
    public class Case
    {
        public int IdCase { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? CourtDate { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public ICollection<Document>? Documents { get; set; }
    }

}