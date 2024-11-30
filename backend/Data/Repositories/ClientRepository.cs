
using backend.Models;

namespace backend.Data.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(MyDbContext context) : base(context) { }
    }
}