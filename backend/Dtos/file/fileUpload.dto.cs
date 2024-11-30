using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace backend.Dtos
{
    public class FileUploadDto
    {
        public required string Name { get; set; }

        public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;

        public required IFormFile File { get; set; }

        public ICollection<EmbeddingDto>? Embeddings { get; set; }
    }
}
