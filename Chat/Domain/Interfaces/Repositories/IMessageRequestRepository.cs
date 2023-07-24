using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IMessageRequestRepository : IRepository<MessageRequest>
    {
        bool ExistsRequest(int requesterId, int receiverId);

        void CreateRequest(int requesterId, int receiverId, string? message);

        List<MessageRequest> GetRequestsByUser(int userId);
    }
}
