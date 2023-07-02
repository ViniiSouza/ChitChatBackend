namespace Chat.Application.DTOs
{
    public class MessagePermissionDTO
    {
        public UserDTO Sender { get; set; }

        public int SenderId { get; set; }

        public UserDTO Receiver { get; set; }

        public int ReceiverId { get; set; }
    }
}
