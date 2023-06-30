using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Extensions;
using Chat.Infra.Data;
using Chat.Security;

namespace Chat.Application.Services
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public AuthenticationAppService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public string? Login(UserLoginDTO dto)
        {
            dto.Password = dto.Password.ToShaHash();
            if (!_unitOfWork.UserRepository.IsUserValid(dto.UserName, dto.Password))
            {
                return null;
            }
            var entity = _mapper.Map<User>(dto);

            _unitOfWork.UserRepository.SetLastLogin(dto.UserName);
            _unitOfWork.Save();
            return TokenService.GenerateToken(entity);
        }

        public string? Register(CreateUserDTO dto)
        {
            if (_unitOfWork.UserRepository.GetByUserName(dto.UserName) != null)
            {
                return null;
            }
            var entity = _mapper.Map<User>(dto);
            entity.Password = entity.Password.ToShaHash();
            _unitOfWork.UserRepository.Create(entity);
            _unitOfWork.Save();
            _unitOfWork.FreeInstance(entity);
            return Login(new(dto.UserName, dto.Password));
        }
    }
}
