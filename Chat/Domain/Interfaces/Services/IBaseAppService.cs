namespace Chat.Domain.Interfaces.Services
{
    public interface IBaseAppService<TDto, TEntity>
    {
        public TDto GetById(int id);
        public IEnumerable<TDto> GetAll();
        public void Create(TDto dto);
        public void Update(TDto dto);
        public void Delete(TDto dto);
    }
}
