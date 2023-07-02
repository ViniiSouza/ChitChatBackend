using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public virtual T? GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsNoTracking().Where(where => where.Id == id);
            
            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.FirstOrDefault();

        }

        public virtual void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
