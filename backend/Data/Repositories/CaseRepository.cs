
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories
{
    public class CaseRepository : BaseRepository<Case>, ICaseRepository
    {
        public CaseRepository(MyDbContext context) : base(context) { }

        public async Task<IEnumerable<Case>> GetAllCasesForAUserIdAsync(int id)
        {
            return await _context.Cases
                .Where(c => c.AssignedUserId == id)
                .Include(c => c.Client)
                .Include(c => c.Documents)
                .ThenInclude(d => d.File)
                .ToListAsync();
        }

        public async Task<bool> ClientExistsAsync(int clientId)
        {
            return await _context.Clients.AnyAsync(c => c.IdClient == clientId);
        }
    }
}