using AutoMapper;
using Onion.Domains.Entities;
using OnionStructure.ViewModel;

namespace OnionStructure.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserModel, User>().ForMember(dest => dest.DisplayName,y=>y.MapFrom(src => src.FullName));
        }
    }
}
