using Application.Commands;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Utils;
using MediatR;

namespace Application.CommandsHandler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostDto>
    {
        private readonly IPostRepository _postRepository;
        private IList<string> InvalidRequestMessage { get; set; }

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
            InvalidRequestMessage = new List<string>();
        }
        public async Task<CreatePostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            bool userReachedPostLimit = await _postRepository.VerifyUsersPostsToday(request.UserId);

            if (userReachedPostLimit)
            {
                InvalidRequestMessage.Add("User reached limit of 5 daily posts.");
                return new CreatePostDto { Post = null, InvalidRequestMessage = InvalidRequestMessage };
            }
            else
            {
                if(await IsValidRequest(request)) 
                {
                    return await CreateNewPost(request, cancellationToken);
                }
                else
                {
                    return new CreatePostDto { Post = null, InvalidRequestMessage = InvalidRequestMessage };
                }
            }
        }

        private async Task<CreatePostDto> CreateNewPost(CreatePostCommand request, CancellationToken cancellationToken)
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

            return new CreatePostDto { Post = post };
        }
        public async Task<bool> IsValidRequest(CreatePostCommand request)
        {
            bool isValidPost = IsValidPost(request);
            bool isValidQuote = await IsValidQuote(request);
            bool isValidRepost = await IsValidRepost(request);

            return isValidPost ^ isValidQuote ^ isValidRepost;
        }

        private bool IsValidPost(CreatePostCommand request)
        {
            if (request.IsQUote)
            {
                InvalidRequestMessage.Add("Param IsQuote must be false to create a post.");
            }

            if (request.IsRepost)
            {
                InvalidRequestMessage.Add("Param IsRepost must be false to create a post.");
            }

            if (request.Quote != null)
            {
                InvalidRequestMessage.Add("Param Quote must be null to create a post.");
            }

            if (request.OriginalPostId != null)
            {
                InvalidRequestMessage.Add("Param OriginalPostId must be null to create a post.");
            }

            if (request.PostContent != null && request.PostContent.Length > Constants.POST_CONTENT_CHAR_LIMIT)
            {
                InvalidRequestMessage.Add("Posts can have a maximum of 777 characters.");
            }

            return request.IsQUote == false && request.IsRepost == false && request.Quote == null && request.OriginalPostId == null && request.PostContent != null && request.PostContent.Length <= Constants.POST_CONTENT_CHAR_LIMIT;
        }

        private async Task<bool> IsValidRepost(CreatePostCommand request)
        {
            if (request.IsQUote)
            {
                InvalidRequestMessage.Add("Param IsQuote must be false when IsRepost is true.");
            }

            if (request.Quote != null)
            {
                InvalidRequestMessage.Add("Param Quote must be null when IsRepost is true.");
            }

            if (request.IsQUote && request.OriginalPostId == null)
            {
                InvalidRequestMessage.Add("Param OriginalPostId must be defined when IsRepost is true.");
            }

            bool isValidRepost = request.IsRepost && request.OriginalPostId != null && request.Quote == null && request.IsQUote == false;

            if (isValidRepost && request.OriginalPostId != null)
            {
                bool originalPostIsRepost = await _postRepository.VerifyPostIsRepost((int)request.OriginalPostId);

                if (originalPostIsRepost)
                {
                    isValidRepost = false;
                    InvalidRequestMessage.Add("You can't repost another repost.");
                }
            }

            return isValidRepost;
        }

        private async Task<bool> IsValidQuote(CreatePostCommand request)
        {
            if (request.IsRepost)
            {
                InvalidRequestMessage.Add("Param IsRepost must be false when IsQUote is true.");
            }

            if (request.Quote == null)
            {
                InvalidRequestMessage.Add("Param Quote must be defined when IsQUote is true.");
            }

            if (request.IsQUote && request.OriginalPostId == null)
            {
                InvalidRequestMessage.Add("Param OriginalPostId must be defined when IsQUote is true.");
            }

            bool isValidQuote = request.IsQUote && request.OriginalPostId != null && request.Quote != null && request.IsRepost == false;

            if (isValidQuote && request.OriginalPostId != null)
            {
                bool originalPostIsQuote = await _postRepository.VerifyPostIsQuote((int)request.OriginalPostId);

                if (originalPostIsQuote)
                {
                    isValidQuote = false;
                    InvalidRequestMessage.Add("You can't quote another quote.");
                }
            }

            return isValidQuote;
        }

        private static Post CreatePost(CreatePostCommand request) => new(request.UserId, request.PostContent!);
        private static Post CreateRepost(CreatePostCommand request) => new(request.UserId, request.OriginalPostId!, request.IsRepost);
        private static Post CreateQuote(CreatePostCommand request) => new(request.UserId, request.OriginalPostId!, request.IsQUote, request.Quote);
    }
}
