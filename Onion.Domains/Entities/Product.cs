using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Domains.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; } = string.Empty;
    }
}
