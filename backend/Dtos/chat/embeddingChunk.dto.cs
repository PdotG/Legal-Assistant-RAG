namespace backend.Dtos
{
    public class EmbeddingChunk
    {
        public string PlainText { get; set; }
        public float CosineSimilarity { get; set; }
    }
}