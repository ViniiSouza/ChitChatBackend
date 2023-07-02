using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.HasIndex(prop => prop.UserName).IsUnique();

            builder.Property(prop => prop.Name).HasMaxLength(40);
            builder.Property(prop => prop.UserName).HasMaxLength(15);

            builder.HasMany(prop => prop.RequestsReceived)
                .WithOne(prop => prop.Receiver)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(prop => prop.RequestsSolicited)
                .WithOne(prop => prop.Requester)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(prop => prop.MessagesSent)
                .WithOne(prop => prop.Sender)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(prop => prop.Contacts)
                .WithOne(prop => prop.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(prop => prop.MessagePermissions)
                .WithOne(prop => prop.Sender)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(prop => prop.Conversations)
                .WithOne(prop => prop.User)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
