using Application.Commands;
using Application.Queries;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Controllers.Requests;

namespace WebApi.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("{id}/metadata")]
        [SwaggerOperation(Summary = "Returns metadata for a user specified by Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Metadata was found for the specified Id.", Type = typeof(UserMetadataDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "The user does not have permission to make this request.", Type = typeof(UserMetadataDto))]
        public async Task<IActionResult> GetUserData([FromRoute] int id)
        {
            if(id != Constants.MOCKED_CURRENT_USERID)
            {
                return Unauthorized();
            }
            else
            {
                var getUserMetadataQuery= new GetUserMetadataQuery(id);
                var userMetadata = await _mediator.Send(getUserMetadataQuery);

                return Ok(userMetadata);
            }
        }

        [HttpGet("{id}/posts")]
        [SwaggerOperation(Summary = "Returns the posts of a user specified by Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "List containing user posts.", Type = typeof(IList<Post>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "User has not yet made a post or the \"page\" parameter entered exceeds the number of posts, in this case an empty list is returned.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "The user does not have permission to make this request.", Type = typeof(UserMetadataDto))]
        public async Task<IActionResult> GetUserPosts([FromRoute] int id, [FromQuery] int page)
        {
            if (id != Constants.MOCKED_CURRENT_USERID)
            {
                return Unauthorized();
            }
            else
            {
                var getAllPostsCommand = new GetAllPostsQuery(id, page, Constants.USER_POSTS_PER_PAGE, true, null);
                var posts = await _mediator.Send(getAllPostsCommand);

                return posts.Any() ? Ok(posts) : NoContent();
            }
        }

        [HttpPost("{id}/post")]
        [SwaggerOperation(Summary = "Create a post associated to the User. The created post is returned.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Post created.", Type = typeof(Post))]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            if (request.IsValidRequest())
            {
                var createPostCommand = new CreatePostCommand(Constants.MOCKED_CURRENT_USERID, 
                    request.PostContent,
                    request.IsRepost, 
                    request.IsQUote, 
                    request.Quote, 
                    request.OriginalPostId);

                var result = await _mediator.Send(createPostCommand);

                return Ok(result);
            }
            else
            {
                return BadRequest(request.InvalidRequestMessage);
            }
        }
    }
}
