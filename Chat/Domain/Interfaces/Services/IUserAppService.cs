using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Services
{
    public interface IUserAppService : IBaseAppService<UserDTO, User>
    {
        string? RequestMessage(string requesterUsername, string receiverUsername, string message);

        List<UserSimpleDTO> GetContactsByUser(string userName);

        bool RemoveContact(string userName, int targetContactId);

        UserSearchDTO SearchUser(string requester, string targetUser);
    }
}
