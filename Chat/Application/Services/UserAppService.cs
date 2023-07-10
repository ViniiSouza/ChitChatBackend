using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Infra.Data;
using Chat.Utils.Enums;

namespace Chat.Application.Services
{
    public class UserAppService : BaseAppService<UserDTO, User>, IUserAppService
    {
        public UserAppService(IMapper mapper, UnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public string? RequestMessage(string requesterUsername, MessagePermissionCreateDTO dto)
        {
            var requester = _unitOfWork.UserRepository.GetByUserName(requesterUsername);
            var receiver = _unitOfWork.UserRepository.GetByUserName(dto.Receiver);
            if (_unitOfWork.MessageRequestRepository.ExistsRequest(requester.Id, receiver.Id))
            {
                return "A message request already exists for this user!";
            }

            _unitOfWork.MessageRequestRepository.CreateRequest(requester.Id, receiver.Id, dto.Message);
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

        public UserSearchDTO SearchUser(string requester, string targetUser)
        {
            if (requester == targetUser)
            {
                throw new InvalidOperationException("You cannot invite yourself!");
            }

            var user = _unitOfWork.UserRepository.GetByUserName(requester);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            var target = _unitOfWork.UserRepository.GetByUserName(targetUser);

            if (target == null)
            {
                throw new InvalidOperationException("User not found!");
            }

            var dto = _mapper.Map<UserSearchDTO>(target);

            if (_unitOfWork.MessagePermissionRepository.CanUserMessage(user.Id, target.Id))
            {
                dto.Type = _unitOfWork.ConversationRepository.ExistsPrivateConversation(user.Id, target.Id) ? ESearchUserType.AlreadyHasChat : ESearchUserType.HasPermission;
            }
            else if (target.IsPublicProfile)
            {
                dto.Type = ESearchUserType.PublicProfile;
                
            }
            else
            {
                dto.Type = _unitOfWork.MessageRequestRepository.ExistsRequest(user.Id, target.Id) ? ESearchUserType.AlreadySentInvite : ESearchUserType.PrivateProfile;
            }

            return dto;
        }
    }
}
