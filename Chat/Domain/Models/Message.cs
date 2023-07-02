using Chat.Utils.Enums;

namespace Chat.Domain.Models
{
    public class Message : EntityBase
    {
        public EMessageAction Action { get; set; }

        public string Content { get; set; }

        public User Sender { get; set; }

        public int SenderId { get; set; }

        public Conversation Chat { get; set; }

        public int ChatId { get; set; }
    }
}
