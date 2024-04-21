using Onion.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Datas.Abstract
{
    public interface IUnitOfWork
    {
        //Responsitory<User> ResponsitoryUser { get; }
        //Responsitory<Room> ResponsitoryRoom { get; }
        //Responsitory<RoomLevel> ResponsitoryRoomLevel { get; }
        //Responsitory<RoomLanguage> ResponsitoryRoomLanguage { get; }
        Task<bool> Complete();
        bool HasChanges();
        IRepositoryGeneric<T> GetRepository<T>() where T : class;
        Task BeginTransaction();
        Task CommitAsync();
        Task RollBackTransaction();
        Task<int> SaveChangeAsync();

        TRepository GetRepository<TRepository, TEntity>() where TRepository : class,
            IRepositoryGeneric<TEntity> where TEntity : class;
        TService GetService<TService, TEntity>() where TService : class;

    }
}
