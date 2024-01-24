using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.MySql.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlContext _context;

        public UserRepository(MySqlContext context)
        {
            _context = context;
        }

        public async Task<UserMetadataDto?> GetUserMetadata(int userId, CancellationToken cancellationToken)
        {
            UserMetadataDto? userMetadata = null;

            var user = await this._context.Users
                .Include(user => user.Posts)
                .Where(user => user.Id.Equals(userId)).SingleOrDefaultAsync();

            if (user != default)
            {
                userMetadata = new UserMetadataDto(
                     user.Posts.Count(),
                     user.UserName,
                     user.CreatedAt
                  );
            }

            return userMetadata;
        }

        public async Task<User?> GetUserById(int userId) => await this._context.Users.AsNoTracking()
            .Where(user => user.Id == userId).FirstOrDefaultAsync();
    }
}
