using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class MessageSimpleDTO
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }

        public string Content { get; set; }

        public string SenderName { get; set; }

        public DateTime SendingTime { get; set; }

        public bool OwnMessage { get; set; }
    }
}
