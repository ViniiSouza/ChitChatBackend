using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Infra.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext context) : base(context)
        {
        }

        public bool IsUserValid(string username, string password)
        {
            return _context.Set<User>().Any(where => where.UserName == username.ToLower() && where.Password == password);
        }

        public override IEnumerable<User> GetAll()
        {
            var result = _context.Set<User>().AsNoTracking().ToList();
            foreach (var user in result)
            {
                user.Password = "";
            }
            return result;
        }

        public override User? GetById(int id, params Expression<Func<User, object>>[] includes)
        {
            // includes is not being used here
            var result = _context.Set<User>().AsNoTracking().Where(where => where.Id == id).FirstOrDefault();
            result.Password = "";
            return result;
        }

        public override void Create(User entity)
        {
            entity.LastLogin = DateTime.Now;
            entity.UserName = entity.UserName.ToLower();
            base.Create(entity);
        }

        public User? GetByUserName(string username)
        {
            var result = _context.Set<User>().AsNoTracking().Where(where => where.UserName == username.ToLower()).FirstOrDefault();
            if (result == null) return null;
            result.Password = null;
            return result;
        }

        public void SetLastLogin(string username)
        {
            var entity = GetByUserName(username);
            entity.LastLogin = DateTime.Now;
            _context.Attach(entity);
            _context.Entry(entity).Property(p => p.LastLogin).IsModified = true;
        }
    }
}
