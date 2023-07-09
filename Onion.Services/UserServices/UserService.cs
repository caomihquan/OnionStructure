using Onion.Datas.Abstract;
using Onion.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Services.UserServices
{
    public class UserService : IUserService
    {
        IResponsitory<User> _user;
        IResponsitory<UserToken> _userToken;

        public UserService(IResponsitory<User> user, IResponsitory<UserToken> userToken)
        {
            _user = user;
            _userToken = userToken;
        }

        public async Task<User> CheckLogin(string username, string password)
        {
            return await _user.GetSingle(x => x.UserName == username && x.Password == password);
        }

        public async Task<User> FindByUserName(string username)
        {
            return await _user.GetSingle(x => x.UserName == username);

        }

        public async Task SaveToken(UserToken userToken)
        {
            await _userToken.Insert(userToken);
            await _userToken.Commit();
        }

        public async Task<UserToken> CheckRefreshToken(string code)
        {
            return await _userToken.GetSingle(x => x.CodeRefreshToken == code);
        }

        public async Task<User> getUserByID(string userID)
        {
            return await _user.GetSingle(x => x.UserID == userID);
        }
    }
}
