namespace Chat.Domain.Models
{
    public class UserContact : EntityBase
    {
        public User User { get; set; }

        public int UserId { get; set; }

        public User Contact { get; set; }

        public int ContactId { get; set; }
    }
}
