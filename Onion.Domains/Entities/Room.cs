using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Onion.Domains.Entities
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public int CountMember { get; set; }
        public int MaximumMember { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User User { get; set; }
        public Guid UserID { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
        public RoomLanguage Language { get; set; }
        public string LanguageCode { get; set; }
        public RoomLevel Level { get; set; }
        public string LevelCode { get; set; }
    }


    public class Connection
    {
        public Connection() { }
        public Connection(string connectionId, string userName)
        {
            ConnectionId = connectionId;
            UserName = userName;
        }
        [Key]
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }
}
