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
        }
    }
}
