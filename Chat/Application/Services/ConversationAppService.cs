using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Chat.Domain.Models;
using Chat.Infra.Data;
using Chat.Utils.Enums;

namespace Chat.Application.Services
{
    public class ConversationAppService : BaseAppService<ConversationDTO, Conversation>, IConversationAppService
    {
        public ConversationAppService(IMapper mapper, UnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public List<ConversationSimpleDTO> LoadConversationsByUser(string username)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(username); // validação
            var conversations = _unitOfWork.ConversationRepository.GetAllSimpleByUser(user.Id).Select(select => new ConversationSimpleDTO()
            {
                Id = select.Id,
                Title = select.Participants.FirstOrDefault(where => where.UserId != user.Id).User.Name, // this will change when implemented group chats
                LastMessage = _mapper.Map<MessageSimpleDTO>(select.LastMessage),
                Type = select.Type
            }).ToList();

            return conversations.OrderByDescending(order => order.LastMessage?.SendingTime).ToList();
        }

        public ConversationSimpleDTO CreatePrivateByRequest(int messageRequestId, string userName)
        {
            var messageRequest = _unitOfWork.MessageRequestRepository.GetById(messageRequestId, include => include.Requester, include => include.Receiver);
            if (messageRequest is null)
            {
                throw new InvalidOperationException("Request not found!");
            }

            var requester = messageRequest.Requester;
            var receiver = messageRequest.Receiver;

            if (userName != receiver.UserName)
            {
                throw new InvalidOperationException("You can't accept this request!");
            }

            SetMessagePermission(requester.Id, receiver.Id);

            if (_unitOfWork.ConversationRepository.ExistsPrivateConversation(requester.Id, receiver.Id))
            {
                throw new InvalidOperationException("There is already a chat between the users!");
            }

            var conversation = _unitOfWork.ConversationRepository.Create(new List<User_Conversation>(), EChatType.Private);

            _unitOfWork.Save();

            var participants = new List<User_Conversation>()
            {
                new User_Conversation(requester.Id, conversation.Id),
                new User_Conversation(receiver.Id, conversation.Id)
            };

            _unitOfWork.User_ConversationRepository.VinculateParticipants(participants);

            _unitOfWork.Save();

            Message creationMessage = new Message()
            {
                Action = EMessageAction.Creation,
                Content = "created",
                CreationDate = DateTime.Now,
                SenderId = requester.Id,
                ChatId = conversation.Id
            };

            Message firstMessage = new Message()
            {
                Action = EMessageAction.Content,
                Content = messageRequest.CustomInvite is not null ? messageRequest.CustomInvite : "Hello!",
                CreationDate = DateTime.Now,
                SenderId = requester.Id,
                ChatId = conversation.Id
            };

            _unitOfWork.MessageRepository.Create(creationMessage);
            _unitOfWork.MessageRepository.Create(firstMessage);

            _unitOfWork.Save();

            _unitOfWork.ConversationRepository.DetachInstance(conversation);

            _unitOfWork.ConversationRepository.UpdateLastMessage(conversation.Id, firstMessage.Id);

            _unitOfWork.Save();

            _unitOfWork.MessageRequestRepository.Delete(messageRequest);

            _unitOfWork.Save();

            conversation.LastMessage = firstMessage;

            var result = _mapper.Map<ConversationSimpleDTO>(conversation);

            result.LastMessage.MessageSender = EMessageSender.Other;
            
            return result;
        }

        public ConversationSimpleDTO CreateAllowedPrivate(ConversationCreateDTO dto, string userName)
        {
            var creator = _unitOfWork.UserRepository.GetByUserName(userName);

            if (creator == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            var receiver = _unitOfWork.UserRepository.GetByUserName(dto.Receiver);

            if (receiver == null)
            {
                throw new InvalidOperationException("Receiver not found. Try again!");
            }

            if (!receiver.IsPublicProfile && !_unitOfWork.MessagePermissionRepository.CanUserMessage(creator.Id, receiver.Id))
            {
                throw new InvalidOperationException("You don't have permission to chat this user. Try to send a message request!");
            }

            SetMessagePermission(creator.Id, receiver.Id);

            if (_unitOfWork.ConversationRepository.ExistsPrivateConversation(creator.Id, receiver.Id))
            {
                throw new InvalidOperationException("There is already a chat between the users!");
            }

            var conversation = _unitOfWork.ConversationRepository.Create(new List<User_Conversation>(), EChatType.Private);

            _unitOfWork.Save();

            var participants = new List<User_Conversation>()
            {
                new User_Conversation(creator.Id, conversation.Id),
                new User_Conversation(receiver.Id, conversation.Id)
            };

            _unitOfWork.User_ConversationRepository.VinculateParticipants(participants);

            _unitOfWork.Save();

            Message creationMessage = new Message()
            {
                Action = EMessageAction.Creation,
                Content = "created",
                CreationDate = DateTime.Now,
                SenderId = creator.Id,
                ChatId = conversation.Id
            };

            Message firstMessage = new Message()
            {
                Action = EMessageAction.Content,
                Content = dto.FirstMessage,
                CreationDate = DateTime.Now,
                SenderId = creator.Id,
                ChatId = conversation.Id
            };

            _unitOfWork.MessageRepository.Create(creationMessage);
            _unitOfWork.MessageRepository.Create(firstMessage);

            _unitOfWork.Save();

            _unitOfWork.ConversationRepository.DetachInstance(conversation);

            _unitOfWork.ConversationRepository.UpdateLastMessage(conversation.Id, firstMessage.Id);

            _unitOfWork.Save();

            conversation.LastMessage = firstMessage;

            var result = _mapper.Map<ConversationSimpleDTO>(conversation);

            result.LastMessage.MessageSender = EMessageSender.Self;
            result.LastMessage.SenderName = creator.Name;
            result.Id = conversation.Id;

            return result;
        }

        private void SetMessagePermission(int firstUserId, int secondUserId)
        {
            if (!_unitOfWork.MessagePermissionRepository.CanUserMessage(firstUserId, secondUserId))
            {
                _unitOfWork.MessagePermissionRepository.Create(new MessagePermission(firstUserId, secondUserId));
            }

            if (!_unitOfWork.MessagePermissionRepository.CanUserMessage(secondUserId, firstUserId))
            {
                _unitOfWork.MessagePermissionRepository.Create(new MessagePermission(secondUserId, firstUserId));
            }
        }
    }
}
