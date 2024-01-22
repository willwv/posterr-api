using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.MySql
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
