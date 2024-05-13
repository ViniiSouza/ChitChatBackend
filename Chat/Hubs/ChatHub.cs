using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConversationAppService _appService;
        private readonly IUserAppService _userAppService;
        // List of users (value) that are listening to specific user status(key)
        private static Dictionary<string, List<string>> Listeners = new();

        public ChatHub(IConversationAppService appService, IUserAppService userAppService)
        {
            _appService = appService;
            _userAppService = userAppService;
        }

        public override async Task OnConnectedAsync()
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

            await AlertUserStatus(new UserStatus(userIdentifier, Utils.Enums.EStatus.Online));

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdentifier = GetUserIdentifier();

            HubConnections.RemoveUserConnection(userIdentifier, Context.ConnectionId);

            // invoke "offline status" to the listeners of user
            // delete the group of listeners after it

            if (!await IsUserOnline(userIdentifier))
            {
                var lastSeen = DateTime.Now;
                _userAppService.UpdateUserLastSeen(userIdentifier, lastSeen);
                await AlertUserStatus(new UserStatus(userIdentifier, Utils.Enums.EStatus.Offline, lastSeen));
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> IsUserOnline(string userName)
        {
            return HubConnections.HasUser(userName);
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

        public async Task AlertUserStatus(UserStatus userStatus)
        {
            if (Listeners.ContainsKey(userStatus.UserName) && Listeners[userStatus.UserName].Any())
            {
                await Clients.Clients(Listeners[userStatus.UserName]).SendAsync("UserStatusChange", userStatus);
            }
        }

        public async Task<UserStatus> ListenToUserStatus(string targetName)
        {
            AddToListeners(targetName, Context.ConnectionId);

            var online = await IsUserOnline(targetName);

            if (online)
            {
                return new UserStatus(targetName, Utils.Enums.EStatus.Online);
            }

            var target = _userAppService.GetUserByUserName(targetName);

            return new UserStatus(targetName, Utils.Enums.EStatus.Offline, target.LastSeen);
        }

        public async Task StopListenToUserStatus(string targetName)
        {
            RemoveFromListeners(targetName, Context.ConnectionId);
        }

        public async Task<UserStatus> ListenToUserStatusByPrivateChat(int chatId)
        {
            var userName = _appService.GetUserFromPrivate(chatId, GetUserIdentifier());

            return await ListenToUserStatus(userName);
        }

        public async Task StopListenToUserStatusByPrivateChat(int chatId)
        {
            var userName = _appService.GetUserFromPrivate(chatId, GetUserIdentifier());

            await StopListenToUserStatus(userName);
        }

        private string GetUserIdentifier()
        {
            return Context.User.FindFirstValue(ClaimTypes.Name);
        }

        private static void AddToListeners(string key, string value)
        {
            if (Listeners.ContainsKey(key))
                Listeners[key].Add(value);
            else
                Listeners.Add(key, new List<string> { value });
        }

        private static void RemoveFromListeners(string key, string value)
        {
            if (Listeners.ContainsKey(key))
            {
                Listeners[key].Remove(value);
                if (!Listeners[key].Any())
                {
                    Listeners.Remove(key);
                }
            }
        }
    }
}
