﻿namespace Chat.Domain.Models
{
    public class MessagePermission : EntityBase
    {
        public MessagePermission(int senderId, int receiverId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        public MessagePermission()
        {

        }

        public User Sender { get; set; }

        public int SenderId { get; set; }

        public User Receiver { get; set; }

        public int ReceiverId { get; set; }
    }
}
