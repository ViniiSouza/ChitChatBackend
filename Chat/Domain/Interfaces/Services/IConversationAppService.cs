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

        /// <summary>
        /// Validates if the user is a participant of the conversation and if true, return a DTO of the conversation, with the last 20 messages
        /// </summary>
        /// <param name="conversationId">Id of the conversation</param>
        /// <param name="username">Token username</param>
        /// <returns>A DTO of the conversation, with the last 20 messages</returns>
        ConversationDTO GetConversation(int conversationId, string username);

        /// <summary>
        /// Retrieve the messages sent before the <paramref name="messageId"/> one and within the conversation <paramref name="conversationId"/>
        /// </summary>
        /// <param name="conversationId">Id of the conversation</param>
        /// <param name="messageId">Id of the message to filter</param>
        /// <returns>A DTO containing the messages and whether there are messages before those returned</returns>
        ConversationDTO GetBeforeMessage(int conversationId, int messageId);

        /// <summary>
        /// Get private conversation, if exists, between <paramref name="userName"/> and <paramref name="targetUserName"/>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="targetUserName"></param>
        /// <returns>A DTO of the conversation</returns>
        ConversationSimpleDTO? GetSimplePrivate(string userName, string targetUserName);

        MessageSimpleDTO SendMessage(MessageCreateDTO dto, string userName);

        string GetUserFromPrivate(int conversationId, string callerUserName);
    }
}
