using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Content).HasMaxLength(255);

            builder.HasOne(prop => prop.Sender)
                .WithMany(prop => prop.MessagesSent)
                .HasForeignKey(prop => prop.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prop => prop.Receiver)
                .WithMany(prop => prop.ReceivedMessages)
                .HasForeignKey(prop => prop.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prop => prop.Chat)
                .WithMany()
                .HasForeignKey(prop => prop.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
