using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IMessagePermissionRepository : IRepository<MessagePermission>
    {
        bool CanUserMessage(int senderId, int receiverId);
    }
}
