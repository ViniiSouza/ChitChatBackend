using Chat.Application.DTOs;

namespace Chat.Domain.Interfaces.Services
{
    public interface IAuthenticationAppService
    {
        string? Login(UserLoginDTO dto);

        string? Register(CreateUserDTO dto);
    }
}
