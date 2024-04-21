using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Onion.Datas.Abstract;
using Onion.Domains.Entities;

namespace Onion.Datas
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        OnionDbContext _onionDbContext;
        //Responsitory<User> _userResponsitory;
        //Responsitory<Room> _roomResponsitory;
        //Responsitory<RoomLevel> _roomLevelResponsitory;
        //Responsitory<RoomLanguage> _roomLanguageResponsitory;
        private bool disposedValue;
        private IDbContextTransaction _transaction;
        private readonly Dictionary<Type, object> _repositories;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(OnionDbContext onionDbContext,IServiceProvider serviceProvider)
        {
            _onionDbContext = onionDbContext;
            _repositories = new Dictionary<Type, object>();
            _serviceProvider = serviceProvider;
        }

        //public Responsitory<User> ResponsitoryUser { get { return _userResponsitory ??= new Responsitory<User>(_onionDbContext); } }
        //public Responsitory<Room> ResponsitoryRoom { get { return _roomResponsitory ??= new Responsitory<Room>(_onionDbContext); } }
        //public Responsitory<RoomLevel> ResponsitoryRoomLevel { get { return _roomLevelResponsitory ??= new Responsitory<RoomLevel>(_onionDbContext); } }
        //public Responsitory<RoomLanguage> ResponsitoryRoomLanguage { get { return _roomLanguageResponsitory ??= new Responsitory<RoomLanguage>(_onionDbContext); } }

        public async Task<bool> Complete()
        {
            return await _onionDbContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _onionDbContext.ChangeTracker.HasChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _onionDbContext?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
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

        public IRepositoryGeneric<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IRepositoryGeneric<T>;
            }
            var repository = new RepositoryGeneric<T>(_onionDbContext);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _onionDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollBackTransaction()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _onionDbContext.SaveChangesAsync();
        }

        TRepository IUnitOfWork.GetRepository<TRepository, TEntity>()
        {
            var repositories = _serviceProvider.GetService<TRepository>();
            if(repositories == null)
            {
                throw new InvalidOperationException("");
            }
            if (repositories is IRepositoryGeneric<TEntity> repoGenection)
            {
                repoGenection.SetDBContext(_onionDbContext);
            }
            else
            {
                throw new InvalidOperationException("");
            }
            return repositories;

        }

        TService IUnitOfWork.GetService<TService, TEntity>()
        {
            var repositories = _serviceProvider.GetService<TService>();
            if(repositories == null)
            {
                throw new InvalidOperationException("");
            }
            return repositories;
        }
    }
}
