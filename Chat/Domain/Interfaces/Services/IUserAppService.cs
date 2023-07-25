using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Services
{
    public interface IUserAppService : IBaseAppService<UserDTO, User>
    {
        UserDTO GetUserByUserName(string userName);

        string? RequestMessage(string requesterUsername, MessagePermissionCreateDTO dto);

        /// <summary>
        /// Get contacts by username
        /// </summary>
        /// <param name="userName">Token username</param>
        /// <returns>The list of contacts of the user</returns>
        List<ContactDTO> GetContactsByUser(string userName);

        bool RemoveContact(string userName, int targetContactId);

        UserSearchDTO SearchUser(string requester, string targetUser);

        DateTime GetUserLastLogin(string userName);

        void UpdateUserLastSeen(string userName, DateTime date);

        List<MessageRequestDTO> GetRequestsByUser(string userName);

        void RefuseRequest(string userName, int requestId);
    }
}
