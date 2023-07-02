using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class MessageSimpleDTO
    {
        public string Content { get; set; }

        public EMessageSender MessageSender { get; set; }

        public string SenderName { get; set; }

        public DateTime SendingTime { get; set; }
    }
}
