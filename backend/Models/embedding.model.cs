using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("embeddings")]
    public class Embedding
    {
        [Key]
        [Column("id_embedding")]
        public int Id { get; set; }

        [Required]
        [Column("id_file")]
        public int FileId { get; set; }

        [ForeignKey("FileId")]
        public File? File { get; set; }

        [Column("embedding")]
        public float[]? Vector { get; set; }

        [Column("chunk_index")]
        public int? ChunkIndex { get; set; }

        [Column("plain_text")]
        public string? PlainText { get; set; }

        [Column("embedding_created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
