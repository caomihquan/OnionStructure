using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Onion.Authentication.Service;
using Onion.Domains.Entities;
using Onion.Domains.Models;
using Onion.Services.UserServices;
using OnionStructure.ViewModel;

namespace OnionStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IUserService _userService;
        ITokenHandler _tokenHandler;

        public AuthenticationController(IUserService userService, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;

        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login(AccountModel model)
        {
            if(model == null)
            {
                return BadRequest("user is not exist");
            }
            var user = await _userService.CheckLogin(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest("user is not invalid");
            }
             (string token,DateTime ExpiredAccessToken) = await _tokenHandler.CreateTokenJwt(user);
            (string code ,string tokenRefresh, DateTime ExpiredRefreshToken) = await _tokenHandler.CreateRefreshTokenJwt(user);

            await _userService.SaveToken(new UserToken
            {
                AccessToken = token,
                RefreshToken = tokenRefresh,
                ExpiredRefreshToken = ExpiredRefreshToken,
                ExpiredAccessToken = ExpiredAccessToken,
                CodeRefreshToken= code,
                UserID = user.UserID
            });
            
            return Ok(new JwtModelToken
            {
                AccessToken = token,
                RefreshToken = tokenRefresh,
                UserName = user.UserName,
                FullName = user.DisplayName
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            var token = await _tokenHandler.ValidateRefreshToken(model.RefeshToken);
            return Ok(token);
        }
    }
}
