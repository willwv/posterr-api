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
            Post newPost;

            if (request.IsRepost)
            {
                newPost = CreateRepost(request);
            }
            else if (request.IsQUote)
            {
                newPost = CreateQuote(request);
            }
            else
            {
                newPost = CreatePost(request);
            }

            var post = await _postRepository.CreatePostAsync(newPost, cancellationToken);

            return post;
        }

        private static Post CreatePost(CreatePostCommand request) => new(request.UserId, request.PostContent);
        private static Post CreateRepost(CreatePostCommand request) => new(request.UserId, request.OriginalPostId!, request.IsRepost);
        private static Post CreateQuote(CreatePostCommand request) => new(request.UserId, request.OriginalPostId!, request.IsQUote, request.Quote);


    }
}
