using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConversationAppService _appService;

        public ChatHub(IConversationAppService appService)
        {
            _appService = appService;
        }

        public override Task OnConnectedAsync()
        {
            if (Context.User == null || !Context.User.Claims.Any() || !Context.User.HasClaim(any => any.Type == ClaimTypes.Name))
            {
                throw new HubException("Unauthorized connection");
            }

            var userIdentifier = GetUserIdentifier();

            if (HubConnections.UserHasConnectionLimit(userIdentifier))
            {
                throw new HubException("User concurrent connections limit exceeded");
            }

            HubConnections.AddUserConnection(userIdentifier, Context.ConnectionId);

            // invoke "online status" to the listeners of user

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdentifier = GetUserIdentifier();

            HubConnections.RemoveUserConnection(userIdentifier, Context.ConnectionId);

            // invoke "offline status" to the listeners of user
            // delete the group of listeners after it

            return base.OnDisconnectedAsync(exception);
        }

        public async Task<MessageSimpleDTO> SendMessage(MessageCreateDTO dto)
        {
            var result = _appService.SendMessage(dto, GetUserIdentifier());
            return result;
        }

        public async Task<ConversationSimpleDTO> CreateConversation(ConversationCreateDTO dto)
        {
            var result = _appService.CreateAllowedPrivate(dto, GetUserIdentifier());
            return result;
        }

        private string GetUserIdentifier()
        {
            return Context.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
