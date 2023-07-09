using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onion.Domains.Entities;
using Onion.Domains.Models;

namespace Onion.Authentication.Service
{
    public interface ITokenHandler
    {
        Task<(string, string, DateTime)> CreateRefreshTokenJwt(User user);
        Task<(string, DateTime)> CreateTokenJwt(User user);
        Task<JwtModelToken> ValidateRefreshToken(string refreshToken);
        Task ValidateToken(TokenValidatedContext context);
    }
}