using ABC.Management.Domain.Entities;
using ABC.SharedKernell;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL
{
    public class UnitOfWork(
        IDbContextFactory<ABCContext> _dbContextFactory) : IDisposable
    {
        private readonly ABCContext _dbContext = _dbContextFactory.CreateDbContext();
        private bool _disposedValue;

        public IRepository<Child> Children =>
            MakeRepository<Child>();

        public IRepository<Antecedent> Antecedents =>
            MakeRepository<Antecedent>();

        public IRepository<Behavior> Behaviors =>
                MakeRepository<Behavior>();

        public IRepository<Consequence> Consequences =>
            MakeRepository<Consequence>();

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

        private RepositoryBase<ABCContext, T> MakeRepository<T>() where T : Entity =>
             new(_dbContext);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}