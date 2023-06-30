using AutoMapper;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Infra.Data;

namespace Chat.Application.Services
{
    public class BaseAppService<TDto, TEntity> : IBaseAppService<TDto, TEntity> where TEntity : EntityBase
    {
        protected readonly IMapper _mapper;
        protected UnitOfWork _unitOfWork;

        public BaseAppService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public virtual void Create(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _unitOfWork.GetRepository<TEntity>().Create(entity);
            _unitOfWork.Save();
        }

        public virtual void Delete(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _unitOfWork.GetRepository<TEntity>()  .Delete(entity);
            _unitOfWork.Save();
        }

        public virtual IEnumerable<TDto> GetAll()
        {
            var entities = _unitOfWork.GetRepository<TEntity>().GetAll();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual TDto GetById(int id)
        {
            var entity = _unitOfWork.GetRepository<TEntity>().GetById(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual void Update(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _unitOfWork.GetRepository<TEntity>().Update(entity);
            _unitOfWork.Save();
        }
    }
}
