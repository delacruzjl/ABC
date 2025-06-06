﻿using ABC.SharedEntityFramework;
using ABC.Management.Domain.Entities;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL
{
    public class UnitOfWork(
        IDbContextFactory<ABCContext> _dbContextFactory) : IUnitOfWork
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
        public IRepository<ChildCondition> ChildConditions =>
            MakeRepository<ChildCondition>();

        public IRepository<Observation> Observations => 
            MakeRepository<Observation>();

        public async Task<int> SaveChangesAsync()
        {
            var count = await _dbContext.SaveChangesAsync();
            //if (count <= 0)
            //{
            //    throw new InvalidOperationException("Nothing saved to database");
            //}

            return count;
        }

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