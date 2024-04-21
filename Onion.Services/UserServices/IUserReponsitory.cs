using Onion.Datas.Abstract;
using Onion.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Services.UserServices
{
    public interface IUserReponsitory : IRepositoryGeneric<User>
    {
        Task<IEnumerable<User>> getUseryUserName(string userNAme);
    }
}
