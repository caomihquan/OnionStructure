using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class User:BaseEntity
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string UserID { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");
        public string DisplayName{ get; set; } = string.Empty;
        public DateTime LastLoggedIn{ get; set; }

    }
}
