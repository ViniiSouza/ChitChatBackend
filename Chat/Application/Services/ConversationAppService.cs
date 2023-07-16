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
    public class ConversationAppService : BaseAppService<ConversationDTO, Conversation>, IConversationAppService
    {
        private readonly IHubContext<ChatHub> _chatHub;
        public ConversationAppService(IMapper mapper, UnitOfWork unitOfWork, IHubContext<ChatHub> chatHub) : base(mapper, unitOfWork)
        {
            _chatHub = chatHub;
        }

        public List<ConversationSimpleDTO> LoadConversationsByUser(string username)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(username);
            var conversations = _unitOfWork.ConversationRepository.GetAllSimpleByUser(user.Id).Select(select => new ConversationSimpleDTO()
            {
                Id = select.Id,
                Title = select.Participants.FirstOrDefault(where => where.UserId != user.Id)?.User?.Name, // this will change when implemented group chats
                LastMessage = _mapper.Map<MessageSimpleDTO>(select.LastMessage, opt => opt.AfterMap((src, dest) => {
                    if (dest is not null
                        && select is not null
                        && select.LastMessage is not null
                        && select.LastMessage.Sender is not null)
                    {
                        dest.OwnMessage = select?.LastMessage?.Sender?.UserName == username;
                    }
                })),
                Type = select.Type
            }).ToList();

            return conversations.OrderByDescending(order => order.LastMessage?.SendingTime).ToList();
        }

        public ConversationDTO GetConversation(int conversationId, string username)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(username);
            var conversationEntity = _unitOfWork.ConversationRepository.GetById(conversationId, user.Id);
            if (conversationEntity == null)
            {
                throw new InvalidOperationException("Chat not found!");
            }
            var messages = _unitOfWork.MessageRepository.GetPaged(conversationEntity.Id);
            var conversation = _mapper.Map<ConversationDTO>(conversationEntity);

            conversation.Messages = messages.Select(select => new MessageDTO()
            {
                Id = select.Id,
                Action = select.Action,
                Content = select.Content,
                SenderName = select.Sender is not null ? select.Sender.Name : "",
                OwnMessage = select.Sender is not null && select.Sender.UserName == username,
                SendingTime = select.CreationDate
            }).ToList();

            return conversation;
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

            result.LastMessage.OwnMessage = false;
            
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

            result.LastMessage.OwnMessage = true;
            result.LastMessage.SenderName = creator.Name;
            result.Id = conversation.Id;
            result.Title = receiver.Name;

            return result;
        }

        public ConversationSimpleDTO? GetSimplePrivate(string userName, string targetUserName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            var target = _unitOfWork.UserRepository.GetByUserName(targetUserName);

            if (target == null)
            {
                throw new InvalidOperationException("User not found. Try again!");
            }

            var chat = _unitOfWork.ConversationRepository.GetPrivateConversation(user.Id, target.Id);

            if (chat == null)
            {
                return null;
            }

            var dto = _mapper.Map<ConversationSimpleDTO>(chat);

            dto.Title = target.Name; // it will be like this while there isn't the "conversation name" feature

            return dto;
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

        public MessageSimpleDTO SendMessage(MessageCreateDTO dto, string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUserName(userName);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username. Try again!");
            }

            var conversation = _unitOfWork.ConversationRepository.GetById(dto.ConversationId, user.Id);

            if (conversation == null)
            {
                throw new InvalidOperationException("Conversation not found!");
            }

            var entity = _mapper.Map<Message>(dto);

            entity.ChatId = conversation.Id;
            entity.SenderId = user.Id;

            _unitOfWork.MessageRepository.Create(entity); // create a specific create for messages that updates this message as the last message
            _unitOfWork.Save();

            _unitOfWork.ConversationRepository.UpdateLastMessage(conversation.Id, entity.Id);
            _unitOfWork.Save();

            var messageDto = new MessageSimpleDTO()
            {
                Id = entity.Id,
                Content = dto.Content,
                ConversationId = conversation.Id,
                OwnMessage = true,
                SenderName = user.Name,
                SendingTime = dto.SendingTime,
            };

            AlertMessage(messageDto, conversation.Id, user.Id);
            messageDto.OwnMessage = true;

            return messageDto;
        }

        private void AlertMessage(MessageSimpleDTO message, int conversationId, int senderId)
        {
            var conversationUsers = _unitOfWork.ConversationRepository.GetUsersFromConversation(conversationId).Where(where => where.Id != senderId).Select(select => select.UserName);

            List<string> receiversIds = new List<string>();

            foreach (var user in conversationUsers)
            {
                if (HubConnections.HasUser(user))
                    receiversIds.AddRange(HubConnections.GetConnectionsByUser(user));
            }

            message.OwnMessage = false;
            _chatHub.Clients.Clients(receiversIds).SendAsync("MessageReceived", message);
        }
    }
}
