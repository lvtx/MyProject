using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BaseRepository<TDbContext> : IDisposable
        where TDbContext : DbContext
    {

        protected TDbContext _dbContext = null;

        public BaseRepository(TDbContext context)
        {
            _dbContext = context;
        }

        public int SaveChanges()
        {
            if (_dbContext != null)
            {
                return _dbContext.SaveChanges();
            }
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            if (_dbContext != null)
            {
                return _dbContext.SaveChangesAsync();
            }
            return Task<int>.FromResult(0);
        }

        #region "Disposable编程模式"

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
