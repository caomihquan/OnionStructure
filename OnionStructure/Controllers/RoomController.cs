using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onion.Domains.Models;
using Onion.Services.RoomServices;
using Onion.Domains.Helper;
using Onion.Domains.Entities;
using Onion.Domains.Extension;

namespace OnionStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseController
    {
        private readonly IRoomServices _roomServices;

        public RoomController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms([FromQuery]PaginationParams roomParams)
        {
            var comments = await _roomServices.GetAllRoomAsync(roomParams);
            Response.AddPaginationHeader(comments.CurrentPage, comments.PageSize, comments.TotalCount, comments.TotalPages);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoom(string name,string LevelCode,string LanguageCode,int numMemeber)
        {
            var room = new Room { RoomName = name, UserID = User.GetUserId(),LanguageCode=LanguageCode,LevelCode=LevelCode,MaximumMember=numMemeber };
            _roomServices.AddRoom(room);
            return Ok(await _roomServices.GetRoomDtoById(room.RoomId));
        }

        [HttpPut]
        public async Task<ActionResult> EditRoom(int id, string editName,string LevelCode, string LanguageCode,int numMemeber)
        {
            var room = await _roomServices.EditRoom(id, editName, LevelCode, LanguageCode, numMemeber);
            if (room != null)
            {
                return Ok(new RoomDto { RoomId = room.RoomId, RoomName = room.RoomName, UserId = room.UserID.ToString() });
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var entity = await _roomServices.DeleteRoom(id);

            if (entity != null)
            {
                return Ok(new RoomDto { RoomId = entity.RoomId, RoomName = entity.RoomName, UserId = entity.UserID.ToString() });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("delete-all")]
        public async Task<ActionResult> DeleteAllRoom()
        {
            await _roomServices.DeleteAllRoom();
            return Ok();//xoa thanh cong
        }

        [HttpGet]
        [Route("get-level")]
        public async Task<IActionResult> GetAllLevel()
        {
            var data = await _roomServices.GetRoomLevel();
            return Ok(data);
        }

        [HttpGet]
        [Route("get-language")]
        public async Task<IActionResult> GetAllLanguage()
        {
            var data = await _roomServices.GetRoomLanguages();
            return Ok(data);
        }



    }
}

