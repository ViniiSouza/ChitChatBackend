using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Services
{
    public interface IUserAppService : IBaseAppService<UserDTO, User>
    {
        string? RequestMessage(string requesterUsername, MessagePermissionCreateDTO dto);

        /// <summary>
        /// Get contacts by username
        /// </summary>
        /// <param name="userName">Token username</param>
        /// <returns>The list of contacts of the user</returns>
        List<UserSimpleDTO> GetContactsByUser(string userName);

        bool RemoveContact(string userName, int targetContactId);

        UserSearchDTO SearchUser(string requester, string targetUser);
    }
}
