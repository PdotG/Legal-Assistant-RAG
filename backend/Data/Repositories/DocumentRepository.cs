
using backend.Models;

namespace backend.Data.Repositories
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(MyDbContext context) : base(context) { }
    }
}