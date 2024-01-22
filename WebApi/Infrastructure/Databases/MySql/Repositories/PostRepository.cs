using Domain.Entities;
using Domain.Interfaces.Repositories;
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

        public async Task<IList<Post>> GetAllPostsAsync(int skip, int take, CancellationToken cancellationToken) => await this._context.Posts.AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        public async Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken)
        {
            await this._context.Posts.AddAsync(post, cancellationToken);
            this._context.SaveChanges();

            return post;
        }
    }
}
