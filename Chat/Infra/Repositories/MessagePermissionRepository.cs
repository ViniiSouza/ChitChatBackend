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
    }
}
