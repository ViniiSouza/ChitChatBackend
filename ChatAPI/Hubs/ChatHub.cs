using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;

        /// <summary>
        /// Invoked when a user send a message. Invoke a "ReceiveMessage" that send all the informations to reproduce it
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="message">Message content</param>
        /// <param name="receiver">Receiver</param>
        /// <param name="time">Hours and minutes the message was sent</param>
        public async Task SendMessage(string sender, string message, string receiver, string time)
        {
            var data = DateTime.Now;
            await Clients.All.SendAsync("ReceiveMessage", sender, message, receiver, time);
        }
    }
}
