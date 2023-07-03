using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Chat.Infra.Data;
using Chat.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace Chat.Infra.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public ConversationRepository(ChatDbContext context) : base(context)
        {
        }

        public Conversation Create(List<User_Conversation> participants, EChatType type)
        {
            var entity = new Conversation()
            {
                CreationDate = DateTime.Now,
                Type = type
            };

            _context.Set<Conversation>().Add(entity);

            return entity;
        }

        public List<Conversation> GetAllSimpleByUser(int userId, params Expression<Func<Conversation, object>>[] includes)
        {
            var query = _context.Set<Conversation>()
                .Include(include => include.LastMessage)
                .Include(include => include.Participants)
                .ThenInclude(include => include.User)
                .Where(where => where.Participants.Any(any => any.UserId == userId));

            if (includes != null && includes.Any() )
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.ToList();
        }

        public void UpdateLastMessage(int conversationId, int messageId)
        {
            var entity = GetById(conversationId);
            entity.LastMessageId = messageId;
            _context.Attach(entity);
            _context.Entry(entity).Property(p => p.LastMessageId).IsModified = true;
        }

        public bool ExistsPrivateConversation(int firstUserId, int secondUserId)
        {
            return _context.Set<Conversation>().Any(where => where.Participants.Any(any => any.UserId == firstUserId) && where.Participants.Any(any => any.UserId == secondUserId));
        }
    }
}
