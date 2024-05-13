using System.Linq.Expressions;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        T? GetById(int id, params Expression<Func<T, object>>[] includes);
        public IEnumerable<T> GetAll();
        public void Create(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public void DetachInstance(T instance);
    }
}
