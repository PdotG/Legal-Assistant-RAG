
using backend.Models;

namespace backend.Data.Repositories
{
    public class CaseRepository : BaseRepository<Case>, ICaseRepository
    {
        public CaseRepository(MyDbContext context) : base(context) { }
    }
}