using backend.Models;

namespace backend.Data.Repositories
{
    public interface IEmbeddingRepository : IBaseRepository<Embedding>
    {
        Task<IEnumerable<Embedding>> GetEmbeddingsByFileIdAsync(int fileId);
    }
}
