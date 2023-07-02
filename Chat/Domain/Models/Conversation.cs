using Chat.Utils.Enums;

namespace Chat.Domain.Models
{
    public class Conversation : EntityBase
    {
        public EChatType Type { get; set; }

        public List<User> Participants { get; set; }

        public string? Title { get; set; }

        public Message? LastMessage { get; set; }

        public int? LastMessageId { get; set; }

        public int LastMessageId { get; set; }
    }
}
