namespace backend.Dtos
{
    public class FileDto
    {
        public int Id { get; set; } // Assuming there's an Id property in the model

        public required string Name { get; set; }

        public DateTime ScrapedAt { get; set; }

        public required byte[] Content { get; set; }

        public ICollection<EmbeddingDto>? Embeddings { get; set; }
    }
}