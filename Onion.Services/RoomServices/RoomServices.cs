using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Onion.Datas;
using Onion.Datas.Abstract;
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
    public class RoomServices : IRoomServices
    {
        private readonly OnionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoomServices(OnionDbContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Room> GetRoomById(int roomId)
        {
            return await _context.Rooms.Include(x => x.Connections).FirstOrDefaultAsync(x => x.RoomId == roomId);
        }

        public async Task<RoomDto> GetRoomDtoById(int roomId)
        {
            return await _context.Rooms.Where(r => r.RoomId == roomId).ProjectTo<RoomDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();//using Microsoft.EntityFrameworkCore;
        }

        public async Task<Room> GetRoomForConnection(string connectionId)
        {
            return await _context.Rooms.Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        /// <summary>
        /// return null no action to del else delete thanh cong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Room> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
            return room;
        }

        public async Task<Room> EditRoom(int id, string newName, string LevelCode, string LanguageCode, int numMemeber)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                room.RoomName = newName;
                room.LevelCode = LevelCode;
                room.LanguageCode = LanguageCode;
                room.MaximumMember = numMemeber;
            }
            return room;
        }

        

        public async Task DeleteAllRoom()
        {
            var list = await _context.Rooms.ToListAsync();
            _context.RemoveRange(list);
        }

        public async Task<PagedList<RoomDto>> GetAllRoomAsync(PaginationParams roomParams)
        {
            var list = _context.Rooms.AsQueryable();
            return await PagedList<RoomDto>.CreateAsync(list.ProjectTo<RoomDto>(_mapper.ConfigurationProvider).AsNoTracking(), roomParams.PageNumber, roomParams.PageSize);
        }

        public async Task UpdateCountMember(int roomId, int count)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                room.CountMember = count;
            }
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<RoomLanguage>> GetRoomLanguages()
        {
            var list = await _unitOfWork.GetRepository<RoomLanguage>().GetAllAsync();
            return list.ToList();
        }

        public async Task<List<RoomLevel>> GetRoomLevel()
        {
            var list = await _unitOfWork.GetRepository<RoomLevel>().GetAllAsync();
            return list.ToList();
        }
    }
}
