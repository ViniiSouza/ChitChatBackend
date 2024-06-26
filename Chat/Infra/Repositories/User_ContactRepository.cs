﻿using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infra.Repositories
{
    public class User_ContactRepository : Repository<UserContact>, IUser_ContactRepository
    {
        public User_ContactRepository(ChatDbContext context) : base(context)
        {
        }

        public List<UserContact> GetContactsByUserId(int userId)
        {
            return _context.Set<UserContact>().Include(include => include.Contact).Where(where => where.UserId == userId).ToList();
        }

        public bool AddUserContact(int userId, int targetId)
        {
            var entity = new UserContact()
            {
                ContactId = targetId,
                UserId = userId,
                CreationDate = DateTime.Now,
            };
            _context.Set<UserContact>().Add(entity);
            return true;
        }

        public bool RemoveUserContact(int userId, int contactId)
        {
            var entity = _context.Set<UserContact>().Where(where => where.UserId == userId && where.ContactId == contactId).FirstOrDefault();
            if (entity == null)
            {
                return false;
            }
            _context.Set<UserContact>().Remove(entity);
            return true;
        }

        public bool UserIsContact(int userId, int targetId)
        {
            return _context.Set<UserContact>().Where(where => where.UserId == userId && where.ContactId == targetId).Any();
        }
    }
}
