using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;

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
            return _context.Set<User>().Select(select => new User()
            {
                Id= select.Id,
                Name = select.Name,
                UserName = select.UserName,
                CreationDate= select.CreationDate
            }).ToList();
        }

        public override User? GetById(int id)
        {
            return _context.Set<User>().Select(select => new User()
            {
                Id= select.Id,
                Name = select.Name,
                UserName = select.UserName,
                CreationDate = select.CreationDate
            }).FirstOrDefault(find => find.Id == id); ;
        }

        public override void Create(User entity)
        {
            entity.LastLogin = DateTime.Now;
            entity.UserName = entity.UserName.ToLower();
            base.Create(entity);
        }

        public User? GetByUserName(string username)
        {
            return _context.Set<User>().Select(select => new User()
            {
                Id = select.Id,
                Name = select.Name,
                UserName = select.UserName,
                CreationDate = select.CreationDate
            }).FirstOrDefault(find => find.UserName == username.ToLower());
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
