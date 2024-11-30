
using backend.Models;

namespace backend.Data.Repositories
{
    public interface ICaseRepository : IBaseRepository<Case> { 
        Task<IEnumerable<Case>> GetAllCasesForAUserIdAsync(int id);
    }
}