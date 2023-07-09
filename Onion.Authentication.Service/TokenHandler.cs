using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Onion.Domains.Entities;
using Onion.Domains.Models;
using Onion.Services.UserServices;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Onion.Authentication.Service
{
    public class TokenHandler : ITokenHandler
    {
        IConfiguration _configuration;
        IUserService _userService;

        public TokenHandler(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<(string,DateTime)> CreateTokenJwt(User user)
        {
            var dateExpiry = DateTime.Now.AddMinutes(int.Parse(_configuration["JWTToken:ExpiryAccessToken"]));

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss,_configuration["JwTToken:Issuer"],ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString(),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud,"Admin",ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Exp,dateExpiry.ToString("yyyy/MM/dd hh:mm:ss"),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                //new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString(),ClaimValueTypes.String,""),
                new Claim(ClaimTypes.Name,Guid.NewGuid().ToString(),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim("UserID",user.UserID.ToString(),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:SignatureKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTToken:Issuer"],
                audience: _configuration["JWTToken:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: dateExpiry,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return await Task.FromResult((tokenString, dateExpiry));
        }


        public async Task<(string,string,DateTime)> CreateRefreshTokenJwt(User user)
        {
            var dateExpiry = DateTime.Now.AddHours(int.Parse(_configuration["JWTToken:ExpiryRefreshToken"]));
            var code = Guid.NewGuid().ToString();
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss,_configuration["JwTToken:Issuer"],ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString(),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Exp,dateExpiry.ToString("yyyy/MM/dd hh:mm:ss"),ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),
                new Claim(ClaimTypes.SerialNumber,code,ClaimValueTypes.String,_configuration["JWTToken:Issuer"]),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:SignatureKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTToken:Issuer"],
                audience: _configuration["JWTToken:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: dateExpiry,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return await Task.FromResult((code, tokenString, dateExpiry));
        }

        public async Task ValidateToken(TokenValidatedContext context)
        {
            var claims = context.Principal.Claims.ToList();

            if(claims.Count < 0)
            {
                context.Fail("This token contains no information");
                return;
            }
            var identity = context.Principal.Identity as ClaimsIdentity;
            if(identity == null || identity.FindFirst(JwtRegisteredClaimNames.Iss) == null)
            {
                context.Fail("This token is not issued by point entry");
                return;
            }
            if(identity.FindFirst("UserName") != null)
            {
                string username = identity.FindFirst("UserName").Value ?? "";
                var user = await _userService.FindByUserName(username);
                if(user == null)
                {
                    context.Fail("This token is invalid for use");
                }
            }

            if(identity.FindFirst(JwtRegisteredClaimNames.Exp) != null){
                var dataExp = identity.FindFirst(JwtRegisteredClaimNames.Exp).Value;
                long ticks = long.Parse(dataExp);
                var dateNow = DateTime.UtcNow;
                var date = DateTimeOffset.FromUnixTimeSeconds(ticks).DateTime;
                //var expDateTime = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;

                var mintues = date.Subtract(dateNow).TotalMinutes;

                if(mintues < 0)
                {
                    context.Fail("This token is expired");
                    return;
                }
            }


        }

        public async Task<JwtModelToken> ValidateRefreshToken(string refreshToken)
        {
            JwtModelToken jwtModel = new();
            var claimsPrinciple = new JwtSecurityTokenHandler().ValidateToken(
                refreshToken, new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:SignatureKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer=false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out _
                );
            if (claimsPrinciple == null) return jwtModel;

            string serialNumber = claimsPrinciple.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;

            if (string.IsNullOrEmpty(serialNumber)) return jwtModel;

            UserToken userToken = await _userService.CheckRefreshToken(serialNumber);

            if(userToken != null)
            {
                User user = await _userService.getUserByID(userToken.UserID);

                (string newAccessToken, DateTime newcreatedDate) = await CreateTokenJwt(user);
                (string newCode,string newRefreshToken, DateTime newcreatedDateRefeshToken) = await CreateRefreshTokenJwt(user);

                return new JwtModelToken
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    FullName = user.DisplayName,
                    UserName = user.UserName,
                };
            }
            return new();
        }
    }
}