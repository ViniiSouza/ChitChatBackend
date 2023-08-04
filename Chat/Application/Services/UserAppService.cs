using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Hubs;
using Chat.Infra.Data;
using Chat.Utils.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Services
{
    public class UserAppService : BaseAppService<UserDTO, User>, IUserAppService
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public UserAppService(IMapper mapper, UnitOfWork unitOfWork, IHubContext<ChatHub> chatHub) : base(mapper, unitOfWork)
        {
            _chatHub = chatHub;
        }

        public UserDTO GetUserByUserName(string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public string? RequestMessage(string requesterUsername, MessagePermissionCreateDTO dto)
        {
            var requester = _unitOfWork.UserRepository.GetByUserName(requesterUsername);
            if (requester == null)
                throw new InvalidOperationException("Invalid username. Try again!");
            var receiver = _unitOfWork.UserRepository.GetByUserName(dto.Receiver);
            if (receiver == null)
                throw new InvalidOperationException("Invalid username. Try again!");
            if (_unitOfWork.MessageRequestRepository.ExistsRequest(requester.Id, receiver.Id))
            {
                return "A message request already exists for this user!";
            }

            _unitOfWork.MessageRequestRepository.CreateRequest(requester.Id, receiver.Id, dto.Message);
            _unitOfWork.Save();

            _chatHub.Clients.Client(dto.Receiver).SendAsync("RequestReceived");

            return null;
        }

        public List<ContactDTO> GetContactsByUser(string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            return _unitOfWork.User_ContactRepository.GetContactsByUserId(user.Id).Select(select => new ContactDTO()
            {
                Id = select.ContactId,
                Name = select.Contact.Name,
                UserName = select.Contact.UserName
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

        public void UpdateUserLastSeen(string userName, DateTime date)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);
            if (user != null)
            {
                _unitOfWork.UserRepository.SetLastSeen(userName, date);
                _unitOfWork.Save();
            }
        }

        public DateTime GetUserLastLogin(string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            return user.LastLogin;
        }

        public List<MessageRequestDTO> GetRequestsByUser(string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            var requests = _unitOfWork.MessageRequestRepository.GetRequestsByUser(user.Id);

            return _mapper.Map<List<MessageRequestDTO>>(requests);
        }

        public void RefuseRequest(string userName, int requestId)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }
            var request = _unitOfWork.MessageRequestRepository.GetById(requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Request not found!");
            }

            if (request.ReceiverId != user.Id)
            {
                throw new InvalidOperationException("You can't refuse this request!");
            }

            _unitOfWork.MessageRequestRepository.Delete(request);
            _unitOfWork.Save();
        }
    }
}
