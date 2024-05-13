using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Infra.Data;

namespace Chat.Application.Services
{
    public class MessagePermissionAppService : BaseAppService<MessagePermissionDTO, MessagePermission>, IMessagePermissionAppService
    {
        public MessagePermissionAppService(IMapper mapper, UnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }
    }
}
