using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.Repositories.GeneralRepository
{
    public interface IUnitOfWork : IDisposable
    {
        // Get the generic repository for any entity
        IGeneralRepository<T> Repository<T>() where T : class;
        Task<IDbContextTransaction> BeginTransactionAsync();
        // Save changes to the database
        Task<int> CompleteAsync();
    }
}
