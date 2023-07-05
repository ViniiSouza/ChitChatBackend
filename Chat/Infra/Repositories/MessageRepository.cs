﻿using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infra.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDbContext context) : base(context)
        {
        }

        public List<Message> GetPaged(int conversationId, int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Invalid page params!");
            }

            int skipCount = (pageNumber - 1) * pageSize;
            return _context.Set<Message>()
                .OrderByDescending(by => by.CreationDate)
                .Include(include => include.Sender)
                .Where(where => where.ChatId == conversationId)
                .Skip(skipCount).Take(pageSize)
                .OrderBy(order => order.CreationDate).ToList();
        }
    }
}
