using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class MessageDTO
    {
        public EMessageAction Action { get; set; }

        public string Content { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public int ChatId { get; set; }
    }
}
