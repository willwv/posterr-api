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

        public async Task<UserMetadataDto> GetUserMetadata(int userId, CancellationToken cancellationToken)
        {
            var userMetadata = await this._context.Users
                .Include(user => user.Posts)
                .Where(user => user.Id.Equals(userId))
                .Select(user => new UserMetadataDto(
                    user.Posts.Count(),
                    user.UserName, 
                    user.CreatedAt
                 )).SingleOrDefaultAsync();

            return userMetadata;
        }
    }
}
