using Chat.Domain.Models;
using Chat.Utils.Enums;
using System.Linq.Expressions;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        List<Conversation> GetAllSimpleByUser(int userId, params Expression<Func<Conversation, object>>[] includes);

        Conversation Create(List<User_Conversation> participants, EChatType type);

        void UpdateLastMessage(int conversationId, int messageId);

        bool ExistsPrivateConversation(int firstUserId, int secondUserId);

        Conversation? GetById(int id, int userId);

        Conversation? GetPrivateConversation(int firstUserId, int secondUserId);

        List<User> GetUsersFromConversation(int conversationId);
    }
}
