using backend.Models;

namespace backend.Data.Repositories
{
    public interface IFileRepository : IBaseRepository<Models.File>
    {
        Task<IEnumerable<Models.File>> GetFilesByUserIdAsync(int userId);
    }
}
