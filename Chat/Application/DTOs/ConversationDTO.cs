using Chat.Domain.Models;
using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class ConversationDTO
    {
        public EChatType Type { get; set; }

        public List<UserDTO> Participants { get; set; }

        public string? Title { get; set; }

        public List<MessageDTO> Messages { get; set; }
    }
}
