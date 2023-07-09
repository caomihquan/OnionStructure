using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Onion.Datas.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Datas
{
    public class Responsitory<T> : IResponsitory<T>,IDisposable where T : class
    {
        OnionDbContext _onionDbContext;
        private bool disposedValue;

        public Responsitory(OnionDbContext onionDbContext)
        {
            _onionDbContext = onionDbContext;
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _onionDbContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression)
        {
            return await _onionDbContext.Set<T>().Where(expression).ToListAsync();
        }
        public async Task<T> GetByID(object id)
        {
            return await _onionDbContext.Set<T>().FindAsync(id);
        }

        public void Delete(T entity)
        {
            EntityEntry entityEntry = _onionDbContext.Entry(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {

            var entities = _onionDbContext.Set<T>().Where(expression).ToList();

            if(entities.Count > 0)
            {
                _onionDbContext.Set<T>().RemoveRange(entities);
            }
        }

        public async  Task Insert(T entity)
        {
            await _onionDbContext.Set<T>().AddAsync(entity);
        }

        public async Task Insert(IEnumerable<T> entity)
        {
            await _onionDbContext.Set<T>().AddRangeAsync(entity);

        }

        public void Update(T entity)
        {
            EntityEntry entityEntry = _onionDbContext.Entry(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public virtual IQueryable<T> Table => _onionDbContext.Set<T>();

        public async Task Commit()
        {
            await _onionDbContext.SaveChangesAsync();
        }

        public async Task<T> GetSingle(Expression<Func<T, bool>> expression)
        {
            return await _onionDbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _onionDbContext.Dispose();
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Responsitory()
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
