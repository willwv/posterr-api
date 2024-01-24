using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases.MySql.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MySqlContext _context;

        public PostRepository(MySqlContext context)
        {
            _context = context;
        }

        public async Task<IList<Post>> GetAllPostsAsync(int userId, bool onlyMine, DateTime? fromDate, int skip, int take, CancellationToken cancellationToken)
        {
            var query = this._context.Posts.AsNoTracking()
            .OrderByDescending(post => post.CreatedAt)
            .AsQueryable();

            if(onlyMine)
            {
                query  = query.Where(post => post.UserId.Equals(userId));
            }

            if(fromDate is not null)
            {
                query  = query.Where(post => post.CreatedAt >= fromDate);
            }

            return await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
        }

        public async Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken)
        {
            await this._context.Posts.AddAsync(post, cancellationToken);
            this._context.SaveChanges();

            return post;
        }

        public async Task<bool> VerifyUsersPostsToday(int userId) => await _context.Posts
                .CountAsync(p => p.UserId == userId && p.CreatedAt.Date == DateTime.Today) >= Constants.POSTS_PER_USER_PER_DAY;

        public async Task<bool> VerifyPostIsRepost(int originalPostId) => await _context.Posts.Where(post => post.Id == originalPostId).AnyAsync(post => post.IsRepost);

        public async Task<bool> VerifyPostIsQuote(int originalPostId) => await _context.Posts.Where(post => post.Id == originalPostId).AnyAsync(post => post.IsQUote);
    }
}
