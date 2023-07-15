using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class MessageCreateDTO
    {
        public EMessageAction Action { get; set; }

        public string Content { get; set; }

        public DateTime SendingTime { get; set; }

        public int ConversationId { get; set; }
    }
}
