using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class ConversationCreateDTO
    {
        public string Receiver { get; set; }

        public string FirstMessage { get; set; }

        public EChatType Type { get; set; }
    }
}
