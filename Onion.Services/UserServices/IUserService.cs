using Onion.Domains.Entities;

namespace Onion.Services.UserServices
{
    public interface IUserService
    {
        Task<User> CheckLogin(string username, string password);
        Task<UserToken> CheckRefreshToken(string code);
        Task<User> FindByUserName(string username);
        Task<User> getUserByID(string userID);
        Task SaveToken(UserToken userToken);
    }
}