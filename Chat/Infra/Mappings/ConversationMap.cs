using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    public class ConversationMap : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Title).HasMaxLength(100);
        }
    }
}
