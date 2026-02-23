using ApexWebAPI.Entities;
using System.Linq.Expressions;

namespace ApexWebAPI.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> Query();
    }
}
