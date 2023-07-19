using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class UserStatus
    {
        public UserStatus(string userName, EStatus status, DateTime lastSeen)
        {
            Status = status;
            LastSeen = lastSeen;
            UserName = userName;
        }

        public UserStatus(string userName, EStatus status)
        {
            Status = status;
            UserName = userName;
        }

        public string UserName { get; set; }

        public EStatus Status { get; set; }

        public DateTime? LastSeen { get; set; }
    }
}
