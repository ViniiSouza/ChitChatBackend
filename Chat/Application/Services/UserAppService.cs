using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Extensions;
using Chat.Infra.Data;
using Chat.Security;

namespace Chat.Application.Services
{
    public class UserAppService : BaseAppService<UserDTO, User>, IUserAppService
    {
        public UserAppService(IMapper mapper, UnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public string? RequestMessage(string requesterUsername, string receiverUsername, string message)
        {
            var requester = _unitOfWork.UserRepository.GetByUserName(requesterUsername);
            var receiver = _unitOfWork.UserRepository.GetByUserName(receiverUsername);
            if (_unitOfWork.MessageRequestRepository.ExistsRequest(requester.Id, receiver.Id))
            {
                return "A message request already exists for this user!";
            }

            _unitOfWork.MessageRequestRepository.CreateRequest(requester.Id, receiver.Id, message);
            _unitOfWork.Save();
            return null;
        }
    }
}
