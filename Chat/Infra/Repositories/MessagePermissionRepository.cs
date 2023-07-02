using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;

namespace Chat.Infra.Repositories
{
    public class MessagePermissionRepository : Repository<MessagePermission>, IMessagePermissionRepository
    {
        public MessagePermissionRepository(ChatDbContext context) : base(context)
        {
        }

        public bool CanUserMessage(int senderId, int receiverId)
        {
            return _context.Set<MessagePermission>().Any(any => any.SenderId == senderId && any.ReceiverId == receiverId);
        }
    }
}
