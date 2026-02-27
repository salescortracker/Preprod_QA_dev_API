using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.Repositories.GeneralRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRMSContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(HRMSContext context)
        {
            _context = context;
        }

        public IGeneralRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGeneralRepository<T>)_repositories[typeof(T)];
            }

            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        // ✅ Added Transaction Support
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
