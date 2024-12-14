namespace backend.Dtos
{
    public class ClientSummaryDto
    {
        public int IdClient { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}