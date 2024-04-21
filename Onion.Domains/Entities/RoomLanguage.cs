using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class RoomLanguage
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public ICollection<Room> Rooms { get; set; }

    }
}
