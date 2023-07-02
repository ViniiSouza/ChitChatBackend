namespace Chat.Domain.Models
{
    public class User_Conversation : EntityBase
    {
        public User_Conversation(int userId, int conversationId)
        {
            UserId = userId;
            ConversationId = conversationId;
        }

        public User_Conversation()
        {

        }

        public User User { get; set; }
        public int UserId { get; set; }
        public Conversation Conversation { get; set; }
        public int ConversationId { get; set; }
    }
}
