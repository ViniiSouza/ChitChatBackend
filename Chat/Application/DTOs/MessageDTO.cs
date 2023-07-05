using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }

        public EMessageAction Action { get; set; }

        public string Content { get; set; }

        public string SenderName { get; set; }

        public DateTime SendingTime { get; set; }

        public bool OwnMessage { get; set; }
    }
}
