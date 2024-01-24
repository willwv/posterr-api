using Application.Queries;
using Domain.Models;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Returns the 10 last posts. It is possible to filter the request to obtain the next 10 posts, obtain only the current user's posts and filter from a specific date.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "List containing users posts.", Type = typeof(IList<PostDto>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Users have not yet made a post or the filters entered are outside the range of posts saved in the database.")]
        public async Task<IActionResult> GetAllPosts([FromQuery] int page = 0, [FromQuery] bool onlyMine = false, [FromQuery] DateTime? fromDate = null)
        {
            var getAllPostsCommand = new GetAllPostsQuery(Constants.MOCKED_CURRENT_USERID, page, Constants.POSTS_PER_PAGE, onlyMine, fromDate);
            var posts = await _mediator.Send(getAllPostsCommand);

            return posts.Any() ? Ok(posts) : NoContent();
        }
    }
}
