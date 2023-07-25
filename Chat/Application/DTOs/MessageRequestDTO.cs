namespace Chat.Application.DTOs
{
    public class MessageRequestDTO
    {
        public int Id { get; set; }

        public UserSimpleDTO Requester { get; set; }

        public int ReceiverId { get; set; }

        public string Message { get; set; }
    }
}
