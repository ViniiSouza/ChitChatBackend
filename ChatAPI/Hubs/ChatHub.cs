using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            if (Context.User == null || !Context.User.Claims.Any() || !Context.User.HasClaim(any => any.Type == ClaimTypes.Name))
            {
                throw new HubException("Unauthorized connection");
            }

            var userIdentifier = Context.User.FindFirstValue(ClaimTypes.Name);

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
            var userIdentifier = Context.User.FindFirstValue(ClaimTypes.Name);

            HubConnections.RemoveUserConnection(userIdentifier, Context.ConnectionId);

            // invoke "offline status" to the listeners of user
            // delete the group of listeners after it

            return base.OnDisconnectedAsync(exception);
        }
    }
}
