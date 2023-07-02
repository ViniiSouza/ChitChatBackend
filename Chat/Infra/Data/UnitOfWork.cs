using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;
using Chat.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infra.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly ChatDbContext _context;

        // repositories
        private Dictionary<Type, object> _repositories;

        private IUserRepository _userRepository;
        private IMessageRequestRepository _messageRequestRepository;
        private IConversationRepository _conversationRepository;
        private IMessagePermissionRepository _messagePermissionRepository;
        private IUser_ConversationRepository _userConversationRepository;
        private IMessageRepository _messageRepository;

        public UnitOfWork(DbContextOptions<ChatDbContext> options)
        {
            _context = new ChatDbContext(options);
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        {
            var entityType = typeof(TEntity);

            if (_repositories.ContainsKey(entityType))
            {
                return (IRepository<TEntity>)_repositories[entityType];
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(entityType, repository);

            return repository;
        }

        public IUserRepository UserRepository {
            get
            {
                if (_userRepository is null)
                    _userRepository = new UserRepository(_context);

                return _userRepository;
            }
        }

        public IMessageRequestRepository MessageRequestRepository
        {
            get
            {
                if (_messageRequestRepository is null)
                    _messageRequestRepository = new MessageRequestRepository(_context);

                return _messageRequestRepository;
            }
        }

        public IConversationRepository ConversationRepository
        {
            get
            {
                if (_conversationRepository is null)
                    _conversationRepository = new ConversationRepository(_context);

                return _conversationRepository;
            }
        }
        public IMessagePermissionRepository MessagePermissionRepository
        {
            get
            {
                if (_messagePermissionRepository is null)
                    _messagePermissionRepository = new MessagePermissionRepository(_context);

                return _messagePermissionRepository;
            }
        }
        public IUser_ConversationRepository User_ConversationRepository
        {
            get
            {
                if (_userConversationRepository is null)
                    _userConversationRepository = new User_ConversationRepository(_context);

                return _userConversationRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (_messageRepository is null)
                    _messageRepository = new MessageRepository(_context);

                return _messageRepository;
            }
        }

        #region General
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAssync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Rollback()
        {
            // doNothing
        }

        public void FreeInstance(object entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        #endregion
    }
}
