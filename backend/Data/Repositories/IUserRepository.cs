using backend.Models;

namespace backend.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserWithFilesAsync(int userId);
    }
}
