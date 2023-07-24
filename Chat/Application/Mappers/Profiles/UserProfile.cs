using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Application.Mappers.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserLoginDTO, User>();
            CreateMap<User, UserSearchDTO>().ForMember(dest => dest.Type, opt => opt.Ignore());
            CreateMap<User, UserSimpleDTO>();
        }
    }
}
