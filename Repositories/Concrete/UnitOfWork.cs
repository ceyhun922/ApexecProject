using ApexWebAPI.Concrete;
using ApexWebAPI.Entities;
using ApexWebAPI.Repositories.Interfaces;

namespace ApexWebAPI.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApexDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ApexDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new Repository<T>(_context);

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> SaveAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() =>
            _context.Dispose();
    }
}
