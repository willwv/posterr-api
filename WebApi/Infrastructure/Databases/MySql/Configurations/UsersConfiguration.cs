using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.MySql.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.HasIndex(user => user.Id)
                .IsUnique();
            builder.HasIndex(user => user.UserName)
                .IsUnique();
            builder.Property(user => user.UserName)
                .IsRequired()
                .HasMaxLength(14);
            builder.HasMany(user => user.Posts)
                .WithOne(post => post.User)
                .HasForeignKey(user => user.Id);
        }
    }
}
