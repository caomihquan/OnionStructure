using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class UserRole
    {
        public string RoleID { get; set; }  
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }    
    }
}
