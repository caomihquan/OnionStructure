using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Onion.Datas;
using Onion.Datas.Abstract;
using Onion.Domains.Entities;
using Onion.Domains.Models;
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
        private readonly OnionDbContext _context;
        private readonly IMapper _mapper;

        public UserService(IResponsitory<User> user, IResponsitory<UserToken> userToken, IMapper mapper, OnionDbContext context)
        {
            _user = user;
            _userToken = userToken;
            _mapper = mapper;
            _context = context;
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

        public async Task<User> getUserByID(Guid userID)
        {
            return await _user.GetSingle(x => x.UserID == userID);
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users.Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)//add CreateMap<AppUser, MemberDto>(); in AutoMapperProfiles
                .SingleOrDefaultAsync();
        }
    }
}
