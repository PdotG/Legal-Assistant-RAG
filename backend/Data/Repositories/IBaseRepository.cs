using System.Linq.Expressions;

namespace backend.Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task Delete(T entity);
        Task SaveChangesAsync();
    }
}
