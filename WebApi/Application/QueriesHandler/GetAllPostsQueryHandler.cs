using Application.Queries;
using Domain.Interfaces.Repositories;
using Domain.Models;
using MediatR;

namespace Application.QueriesHandler
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IList<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IList<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            int skipItens = request.Page * request.ItensPerPage;
            var posts = await _postRepository.GetAllPostsAsync(request.UserId, request.OnlyMine, request.FromDate, skipItens, request.ItensPerPage, cancellationToken);

            var postsDto = posts.Select(post => post.ToPostDto()).ToList();

            return postsDto;
        }
    }
}
