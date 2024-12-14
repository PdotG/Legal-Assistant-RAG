
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(MyDbContext context) : base(context) { }

        public async Task<IEnumerable<Client>> GetClientsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(c => c.IdUser == userId)
                .ToListAsync();
        }
    }
}