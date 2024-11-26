using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories
{
    public class FileRepository : BaseRepository<Models.File>, IFileRepository
    {
        public FileRepository(MyDbContext context) : base(context) { }

        public async Task<IEnumerable<Models.File>> GetFilesByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }
    }
}
