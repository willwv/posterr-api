using Application.Queries;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Utils;
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
            int skipItens = request.Page * request.ItensPerPage;
            var posts = await _postRepository.GetAllPostsAsync(request.UserId, request.OnlyMine, request.FromDate, skipItens, request.ItensPerPage, cancellationToken);
            return posts;
        }
    }
}
