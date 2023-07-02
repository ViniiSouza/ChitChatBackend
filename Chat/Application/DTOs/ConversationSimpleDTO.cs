using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class ConversationSimpleDTO
    {
        public int Id { get; set; }

        public EChatType Type { get; set; }

        public string Title { get; set; }

        public MessageSimpleDTO LastMessage { get; set; }
    }
}
