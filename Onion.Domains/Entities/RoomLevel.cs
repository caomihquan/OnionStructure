using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class RoomLevel
    {
        public string LevelCode { get; set; }
        public string LevelName { get; set; }
        public ICollection<Room> Rooms { get; set; }

    }
}
