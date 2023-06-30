using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;

namespace Chat.Infra.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly ChatDbContext _context;

        public Repository(ChatDbContext context)
        {
            _context = context;
        }

        public virtual void Create(T entity)
        {
            entity.CreationDate = DateTime.Now;
            _context.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T? GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(find => find.Id == id);
        }

        public virtual void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
