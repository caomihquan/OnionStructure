using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class User
    {
        [Required]
        public Guid UserID { get; set; } = new Guid();
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
       
        public string DisplayName{ get; set; } = string.Empty;
        public DateTime LastLoggedIn{ get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;
        public bool Locked { get; set; } = false;// true = locked

        public string? PhotoUrl { get; set; }//Nullable<string>
        public ICollection<Room> Rooms { get; set; }
        public string RoleID { get; set; }
        public UserRole Role { get; set; }

    }
}
