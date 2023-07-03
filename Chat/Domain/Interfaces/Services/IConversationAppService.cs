using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Services
{
    public interface IConversationAppService : IBaseAppService<ConversationDTO, Conversation>
    {
        /// <summary>
        /// Retrieve all user conversations
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A list of conversations from user</returns>
        List<ConversationSimpleDTO> LoadConversationsByUser(string username);

        /// <summary>
        /// Create a one-to-one conversation based on a message request
        /// </summary>
        /// <param name="messageRequestId">Message request id</param>
        /// <param name="userName">Token username</param>
        /// <returns>A DTO with informations of the created conversation</returns>
        ConversationSimpleDTO CreatePrivateByRequest(int messageRequestId, string userName);

        /// <summary>
        /// Create a one-to-one conversation when user has permission to chat the receiver or when the receiver has public profile
        /// </summary>
        /// <param name="dto">DTO</param>
        /// <param name="userName">Token username</param>
        /// <returns>A DTO with informations of the created conversation</returns>
        ConversationSimpleDTO CreateAllowedPrivate(ConversationCreateDTO dto, string userName);
    }
}
