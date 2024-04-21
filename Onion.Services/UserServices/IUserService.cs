using Onion.Domains.Entities;
using Onion.Domains.Models;

namespace Onion.Services.UserServices
{
    public interface IUserService
    {
        Task<User> CheckLogin(string username, string password);
        Task<UserToken> CheckRefreshToken(string code);
        Task<User> FindByUserName(string username);
        Task<User> getUserByID(Guid userID);
        Task SaveToken(UserToken userToken);
        Task<MemberDto> GetMemberAsync(string username);
    }
}