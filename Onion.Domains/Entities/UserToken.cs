using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class UserToken:BaseEntity
    {
        public string AccessToken { get; set; }
        public DateTime ExpiredAccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredRefreshToken { get; set; }
        public string CodeRefreshToken { get; set; }

        public Guid UserID { get; set; }

    }
}
