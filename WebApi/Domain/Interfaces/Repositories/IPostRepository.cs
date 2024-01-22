using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<IList<Post>> GetAllPostsAsync(int skip, int take, CancellationToken cancellationToken);
        Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken);
    }
}
