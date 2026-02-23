using ApexWebAPI.Concrete;
using ApexWebAPI.Entities;
using ApexWebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApexWebAPI.Repositories.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApexDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApexDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);

        public void Update(T entity) =>
            _dbSet.Update(entity);

        public void Remove(T entity) =>
            _dbSet.Remove(entity);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null) =>
            predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);

        public IQueryable<T> Query() =>
            _dbSet.AsQueryable();
    }
}
