using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models;

[Table("documents")]
public class Document
{
    [Key]
    [Column("id_document")]
    public int IdDocument { get; set; }

    [Required]
    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Required]
    [Column("uploaded_date")]
    public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey("Case")]
    [Column("id_case")]
    public int CaseId { get; set; }

    public Case? Case { get; set; }

    [ForeignKey("File")]
    [Column("file_id")]
    public int FileId { get; set; }

    public backend.Models.File? File { get; set; }
}
