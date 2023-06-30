using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    public class MessageRequestMap : IEntityTypeConfiguration<MessageRequest>
    {
        public void Configure(EntityTypeBuilder<MessageRequest> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.HasOne(prop => prop.Requester)
                .WithMany(prop => prop.RequestsSolicited)
                .HasForeignKey(prop => prop.RequesterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prop => prop.Receiver)
                .WithMany(prop => prop.RequestsReceived)
                .HasForeignKey(prop => prop.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
