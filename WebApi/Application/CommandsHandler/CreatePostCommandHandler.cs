using Application.Commands;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.CommandsHandler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var newPost = new Post(request.UserId, request.PostContent, DateTime.UtcNow);

            var post = await _postRepository.CreatePostAsync(newPost, cancellationToken);

            return post;
        }
    }
}
