using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        List<Message> GetPaged(int conversationId, int pageNumber = 1, int pageSize = 20);
    }
}
