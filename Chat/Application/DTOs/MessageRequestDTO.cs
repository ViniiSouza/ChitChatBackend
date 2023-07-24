namespace Chat.Application.DTOs
{
    public class MessageRequestDTO
    {
        public UserSimpleDTO Requester { get; set; }

        public int ReceiverId { get; set; }

        public string Message { get; set; }
    }
}
