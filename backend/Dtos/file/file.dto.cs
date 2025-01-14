namespace backend.Dtos
{
    public class FileDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public DateTime ScrapedAt { get; set; }

        public required string FilePath { get; set; }

        public ICollection<EmbeddingDto>? Embeddings { get; set; }
    }
}