using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infra.Repositories
{
    public class MessageRequestRepository : Repository<MessageRequest>, IMessageRequestRepository
    {
        public MessageRequestRepository(ChatDbContext context) : base(context)
        {
        }

        public bool ExistsRequest(int requesterId, int receiverId)
        {
            return _context.Set<MessageRequest>().Any(any => any.RequesterId == requesterId && any.ReceiverId == receiverId);
        }

        public void CreateRequest(int requesterId, int receiverId, string? message)
        {
            if (requesterId == receiverId)
            {
                throw new Exception("You cannot send a message request to yourself!");
            }
            var entity = new MessageRequest(requesterId, receiverId, message);
            entity.CreationDate = DateTime.Now;
            _context.Set<MessageRequest>().Add(entity);
        }

        public List<MessageRequest> GetRequestsByUser(int userId)
        {
            return _context.Set<MessageRequest>().Include(include => include.Requester).Where(where => where.ReceiverId == userId).ToList();
        }
    }
}
