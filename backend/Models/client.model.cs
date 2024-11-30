using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models;
using Microsoft.EntityFrameworkCore;

[Table("clients")]
[Index(nameof(ContactInformation), IsUnique = true)]
public class Client
{
    [Key]
    [Column("id_client")]
    public int IdClient { get; set; }

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("contact_information")]
    public string ContactInformation { get; set; } = string.Empty;

    [Column("address")]
    [StringLength(250)]
    public string? Address { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    public ICollection<Case>? Cases { get; set; }
}
