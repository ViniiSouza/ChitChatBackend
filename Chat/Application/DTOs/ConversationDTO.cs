using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class ConversationDTO
    {
        public int Id { get; set; }

        public EChatType Type { get; set; }

        public List<UserDTO> Participants { get; set; }

        public string? Title { get; set; }

        public List<MessageDTO> Messages { get; set; }

        public int? ReceiverId { get; set; }

        public bool? IsContact { get; set; }

        public bool HasPreviousMessages { get; set; }
    }
}
