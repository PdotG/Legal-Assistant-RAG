using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories
{
    public class EmbeddingRepository : BaseRepository<Embedding>, IEmbeddingRepository
    {
        public EmbeddingRepository(MyDbContext context) : base(context) { }

        public async Task<IEnumerable<Embedding>> GetEmbeddingsByFileIdAsync(int fileId)
        {
            return await _dbSet
                .Where(e => e.FileId == fileId)
                .ToListAsync();
        }
    }
}
