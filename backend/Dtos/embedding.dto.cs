
namespace backend.Dtos
{
    public class EmbeddingDto
    {
        public int Id { get; set; }

        public int FileId { get; set; }

        public float[]? Vector { get; set; }

        public int? ChunkIndex { get; set; }

        public string? PlainText { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}