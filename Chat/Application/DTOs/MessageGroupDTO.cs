namespace Chat.Application.DTOs
{
    public class MessageGroupDTO
    {
        public DateOnly Date { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
