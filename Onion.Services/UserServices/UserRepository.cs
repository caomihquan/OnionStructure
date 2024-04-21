using Microsoft.EntityFrameworkCore;
using Onion.Datas;
using Onion.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Services.UserServices
{
    public class UserRepository : RepositoryGeneric<User>, IUserReponsitory
    {
        public UserRepository(OnionDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>> getUseryUserName(string userNAme)
        {
            return await _entitiySet.Where(p => p.UserName.Contains(userNAme)).ToListAsync();
        }
    }
}
