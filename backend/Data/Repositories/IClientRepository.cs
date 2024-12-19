
using backend.Models;

namespace backend.Data.Repositories
{
    public interface IClientRepository : IBaseRepository<Client> 
    { 
        Task<IEnumerable<Client>> GetClientsByUserIdAsync(int userId);
    }
}