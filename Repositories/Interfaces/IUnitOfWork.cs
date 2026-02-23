using ApexWebAPI.Entities;

namespace ApexWebAPI.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveAsync();
    }
}
