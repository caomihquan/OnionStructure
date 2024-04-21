using Onion.Domains.Entities;
using Onion.Domains.Helper;
using Onion.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Services.RoomServices
{
    public interface IRoomServices
    {
        Task<List<RoomLanguage>> GetRoomLanguages();
        Task<List<RoomLevel>> GetRoomLevel();
        Task<Room> GetRoomById(int roomId);
        Task<Room> GetRoomForConnection(string connectionId);
        void RemoveConnection(Connection connection);
        void AddRoom(Room room);
        Task<Room> DeleteRoom(int id);
        Task DeleteAllRoom();
        Task<PagedList<RoomDto>> GetAllRoomAsync(PaginationParams roomParams);
        Task<RoomDto> GetRoomDtoById(int roomId);
        Task UpdateCountMember(int roomId, int count);
        Task<bool> Complete();
        Task<Room> EditRoom(int id, string newName, string LevelCode, string LanguageCode, int numMemeber);
    }
}
