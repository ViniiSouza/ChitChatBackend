namespace Chat.Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        public T? GetById(int id);
        public IEnumerable<T> GetAll();
        public void Create(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
