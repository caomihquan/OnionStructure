using Onion.Datas.Abstract;
using Onion.Domains.Entities;

namespace Onion.Datas
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        OnionDbContext _onionDbContext;
        Responsitory<User> _userResponsitory;
        Responsitory<Room> _roomResponsitory;
        Responsitory<RoomLevel> _roomLevelResponsitory;
        Responsitory<RoomLanguage> _roomLanguageResponsitory;
        private bool disposedValue;

        public UnitOfWork(OnionDbContext onionDbContext)
        {
            _onionDbContext = onionDbContext;
        }

        public Responsitory<User> ResponsitoryUser { get { return _userResponsitory ??= new Responsitory<User>(_onionDbContext); } }
        public Responsitory<Room> ResponsitoryRoom { get { return _roomResponsitory ??= new Responsitory<Room>(_onionDbContext); } }
        public Responsitory<RoomLevel> ResponsitoryRoomLevel { get { return _roomLevelResponsitory ??= new Responsitory<RoomLevel>(_onionDbContext); } }
        public Responsitory<RoomLanguage> ResponsitoryRoomLanguage { get { return _roomLanguageResponsitory ??= new Responsitory<RoomLanguage>(_onionDbContext); } }

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
    }
}
