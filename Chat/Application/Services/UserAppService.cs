using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Infra.Data;

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

        public List<UserSimpleDTO> GetContactsByUser(string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            return _unitOfWork.User_ContactRepository.GetContactsByUserId(user.Id).Select(select => new UserSimpleDTO()
            {
                Id = select.ContactId,
                UserName = select.Contact.Name
            }).ToList();
        }

        public bool RemoveContact(string userName, int targetContactId)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            var result = _unitOfWork.User_ContactRepository.RemoveUserContact(user.Id, targetContactId);
            _unitOfWork.Save();
            return result;
        }
    }
}
