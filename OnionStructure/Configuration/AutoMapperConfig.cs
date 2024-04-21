using AutoMapper;
using Onion.Domains.Entities;
using Onion.Domains.Models;
using OnionStructure.ViewModel;

namespace OnionStructure.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserModel, User>().ForMember(dest => dest.DisplayName,y=>y.MapFrom(src => src.FullName));
            CreateMap<User, MemberDto>();
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
