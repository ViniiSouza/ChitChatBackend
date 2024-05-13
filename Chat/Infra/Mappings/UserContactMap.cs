using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infra.Mappings
{
    public class UserContactMap : IEntityTypeConfiguration<UserContact>
    {
        public void Configure(EntityTypeBuilder<UserContact> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.HasOne(prop => prop.User)
                .WithMany(prop => prop.Contacts)
                .HasForeignKey(prop => prop.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prop => prop.Contact)
                .WithMany()
                .HasForeignKey(prop => prop.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
