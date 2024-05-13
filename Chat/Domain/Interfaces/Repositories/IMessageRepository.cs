using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        List<Message> GetPaged(int conversationId, int pageNumber = 1, int pageSize = 20);

        bool HasMessagesBefore(int messageId, int conversationId);

        List<Message> GetBeforeMessage(int conversationId, int messageId, int amount = 20);
    }
}
