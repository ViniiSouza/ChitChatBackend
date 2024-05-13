using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    public class MessagePermissionMap : IEntityTypeConfiguration<MessagePermission>
    {
        public void Configure(EntityTypeBuilder<MessagePermission> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.HasOne(prop => prop.Sender)
                .WithMany(prop => prop.MessagePermissions)
                .HasForeignKey(prop => prop.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prop => prop.Receiver)
                .WithMany()
                .HasForeignKey(prop => prop.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
