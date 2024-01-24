using Application.Queries;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.UnitTests.Shared;
using WebApi.Controllers;

namespace Tests.UnitTests.Controllers
{
    [TestClass]
    public class PostControllerTest
    {

        [TestMethod]
        public async Task GetAllPosts_OkResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<GetAllPostsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(SharedMocks.GetMockedPostsDtoList());
            var controller = new PostsController(mockMediatr.Object);

            var result = await controller.GetAllPosts();

            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetAllPosts_NoContentResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<GetAllPostsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<PostDto>() { });

            var controller = new PostsController(mockMediatr.Object);
            var result = await controller.GetAllPosts();

            Assert.IsTrue(result is NoContentResult);
        }

    }
}
