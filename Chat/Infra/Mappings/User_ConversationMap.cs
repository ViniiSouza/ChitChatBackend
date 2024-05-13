using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    public class User_ConversationMap : IEntityTypeConfiguration<User_Conversation>
    {
        public void Configure(EntityTypeBuilder<User_Conversation> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.HasOne(prop => prop.Conversation)
                .WithMany(prop => prop.Participants)
                .HasForeignKey(prop => prop.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prop => prop.User)
                .WithMany(prop => prop.Conversations)
                .HasForeignKey(prop => prop.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
