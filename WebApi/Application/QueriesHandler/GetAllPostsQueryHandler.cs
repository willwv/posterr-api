using Application.Queries;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.QueriesHandler
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IList<Post>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IList<Post>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllPostsAsync(0, 10, cancellationToken);
            return posts;
        }
    }
}
