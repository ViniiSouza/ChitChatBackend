namespace Chat.Domain.Models
{
    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public List<MessageRequest> RequestsSolicited { get; set; }

        public List<MessageRequest> RequestsReceived { get; set; }

        public DateTime LastLogin { get; set; }

        public List<Message> MessagesSent { get; set; }

        public List<Message> ReceivedMessages { get; set; }
    }
}
