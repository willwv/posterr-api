﻿using Application.Commands;
using Application.Queries;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.UnitTests.Shared;
using WebApi.Controllers;
using WebApi.Controllers.Requests;

namespace Tests.UnitTests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public async Task GetUserData_OkResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<GetUserMetadataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserMetadataDto (10, "Mocked User", DateTime.UtcNow.AddDays(-1)));

            var controller = new UsersController(mockMediatr.Object);

            var result = await controller.GetUserData(Constants.MOCKED_CURRENT_USERID);

            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetUserPosts_OkResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<GetAllPostsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SharedMocks.GetMockedPostsList(Constants.MOCKED_CURRENT_USERID));

            var controller = new UsersController(mockMediatr.Object);

            var result = await controller.GetUserPosts(Constants.MOCKED_CURRENT_USERID, 0);

            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetUserPosts_NoContentResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<GetAllPostsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Post>() { });

            var controller = new UsersController(mockMediatr.Object);

            var result = await controller.GetUserPosts(Constants.MOCKED_CURRENT_USERID, 0);

            Assert.IsTrue(result is NoContentResult);
        }

        [TestMethod]
        public async Task CreatePost_OkResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<CreatePostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreatePostDto
                {
                    Post = new Post
                    {
                        Id = 1,
                        Content = "Mocked Post!",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        IsQUote = false,
                        IsRepost = false,
                        Quote = null,
                        OriginalPostId = null,
                        User = null,
                        UserId = Constants.MOCKED_CURRENT_USERID
                    }
                });

            var controller = new UsersController(mockMediatr.Object);

            var request = new CreatePostRequest
            (
                "Mocked Post!",
                false,
                false,
                null,
                null
            );
            var result = await controller.CreatePost(Constants.MOCKED_CURRENT_USERID, request);

            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public async Task CreatePost_NadRequestResult()
        {
            var mockMediatr = new Mock<IMediator>();
            mockMediatr.Setup(method => method.Send(It.IsAny<CreatePostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreatePostDto
                {
                    Post = null,
                    InvalidRequestMessage = new List<string> { "Your request is invalid!" }
                });

            var controller = new UsersController(mockMediatr.Object);

            var request = new CreatePostRequest
            (
                "Mocked Post!",
                true,
                true,
                "Mocked Quote",
                null
            );
            var result = await controller.CreatePost(Constants.MOCKED_CURRENT_USERID, request);

            Assert.IsTrue(result is BadRequestObjectResult);
        }
    }
}
