using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Repositories;
using System;

namespace CombatAnalysis.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly CombatAnalysisContext _dbContext;

        private GenericRepository<Combat> _repository;
        private bool disposed = false;

        public UnitOfWork(CombatAnalysisContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GenericRepository<Combat> Combats
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new GenericRepository<Combat>(_dbContext);
                }

                return _repository;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
