using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.MySql.Configurations
{
    public class PostsConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(post => post.Id);
            builder.HasIndex(post => post.Id).IsUnique();
            builder.Property(post => post.Content).HasMaxLength(777).IsRequired(false);
            builder.Property(post => post.Quote).HasMaxLength(777).IsRequired(false);
            builder.HasOne(post => post.User)
                .WithMany(user => user.Posts)
                .HasForeignKey(post => post.UserId);
        }
    }
}
