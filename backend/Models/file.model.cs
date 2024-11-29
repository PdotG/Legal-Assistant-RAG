using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("files")]
    public class File
    {
        [Key]
        [Column("id_file")]
        public int Id { get; set; }

        [Required]
        [Column("id_user")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [Column("name")]
        public required string Name { get; set; }

        [Column("scraped_at")]
        public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("file_path")]
        public required string FilePath { get; set; }

        public ICollection<Embedding>? Embeddings { get; set; }
    }
}
