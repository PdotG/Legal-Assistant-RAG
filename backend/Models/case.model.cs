using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models;
using Microsoft.EntityFrameworkCore;

[Table("cases")]
[Index(nameof(Status))]
public class Case
{
    [Key]
    [Column("id_case")]
    public int IdCase { get; set; }

    [Required]
    [Column("title")]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column("status")]
    public string Status { get; set; } = "Open";

    [Required]
    [Column("created_date")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column("updated_date")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    [Column("court_date")]
    public DateTime? CourtDate { get; set; }

    [ForeignKey("Client")]
    [Column("client_id")]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    [ForeignKey("AssignedUser")]
    [Column("assigned_user_id")]
    public int? AssignedUserId { get; set; }

    public User? AssignedUser { get; set; }

    public ICollection<Document>? Documents { get; set; }
}
