﻿namespace Chat.Domain.Models
{
    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsPublicProfile { get; set; }

        public List<MessageRequest> RequestsSolicited { get; set; }

        public List<MessageRequest> RequestsReceived { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime? LastSeen { get; set; }

        public List<Message> MessagesSent { get; set; }

        public List<UserContact> Contacts { get; set; }

        public List<MessagePermission> MessagePermissions { get; set; }

        public List<User_Conversation> Conversations { get; set; }
    }
}
