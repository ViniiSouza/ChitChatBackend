using Chat.Domain.Interfaces.Repositories;
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
    }
}
