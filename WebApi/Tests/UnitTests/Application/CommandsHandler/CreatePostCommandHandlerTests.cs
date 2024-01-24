using Application.Commands;
using Application.CommandsHandler;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;
using Tests.UnitTests.Shared;

namespace Tests.UnitTests.Application.CommandsHandler
{
    [TestClass]
    public class CreatePostCommandHandlerTests
    {
        private Mock<IPostRepository> _postRepository;

        [TestMethod]
        public async Task CreateValidPost()
        {
            var commandHandler = ObterHandler();
            var commandRequest = new CreatePostCommand(
                SharedMocks.MockedCurrentUserId,
                "Mocked Post!",
                false,
                false,
                null,
                null);

            _postRepository.Setup(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsQuote(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsRepost(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post 
            { 
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                Content = commandRequest.PostContent,
                IsQUote = false,
                IsRepost = false,
                UserId = commandRequest.UserId
            });


            var commandResult = await commandHandler.Handle(commandRequest, new CancellationToken());

            Assert.IsNotNull(commandResult);
            Assert.IsNotNull(commandResult.Post);
            Assert.IsNull(commandResult.InvalidRequestMessage);
            _postRepository.Verify(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId), Times.Once);
            _postRepository.Verify(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(method => method.VerifyPostIsQuote(1), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsRepost(1), Times.Never);
        }

        [TestMethod]
        public async Task CreateValidQuote()
        {
            var commandHandler = ObterHandler();
            var commandRequest = new CreatePostCommand(
                SharedMocks.MockedCurrentUserId,
                null,
                false,
                true,
                "Mocked Quote!",
                1);

            _postRepository.Setup(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsQuote(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsRepost(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                IsQUote = true,
                Quote = commandRequest.Quote,
                IsRepost = false,
                UserId = commandRequest.UserId
            });


            var commandResult = await commandHandler.Handle(commandRequest, new CancellationToken());

            Assert.IsNotNull(commandResult);
            Assert.IsNotNull(commandResult.Post);
            Assert.IsNull(commandResult.InvalidRequestMessage);
            _postRepository.Verify(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId), Times.Once);
            _postRepository.Verify(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(method => method.VerifyPostIsQuote(1), Times.Once);
            _postRepository.Verify(method => method.VerifyPostIsRepost(1), Times.Never);
        }

        [TestMethod]
        public async Task CantQuoteOtherQuote()
        {
            var commandHandler = ObterHandler();
            var commandRequest = new CreatePostCommand(
                SharedMocks.MockedCurrentUserId,
                null,
                false,
                true,
                "Mocked Quote!",
                1);

            _postRepository.Setup(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsQuote(1)).ReturnsAsync(true);
            _postRepository.Setup(method => method.VerifyPostIsRepost(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                IsQUote = true,
                Quote = commandRequest.Quote,
                IsRepost = false,
                UserId = commandRequest.UserId
            });


            var commandResult = await commandHandler.Handle(commandRequest, new CancellationToken());

            Assert.IsNotNull(commandResult);
            Assert.IsNull(commandResult.Post);
            Assert.IsNotNull(commandResult.InvalidRequestMessage);
            _postRepository.Verify(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId), Times.Once);
            _postRepository.Verify(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsQuote(1), Times.Once);
            _postRepository.Verify(method => method.VerifyPostIsRepost(1), Times.Never);
        }

        [TestMethod]
        public async Task CreateValidRepost()
        {
            var commandHandler = ObterHandler();
            var commandRequest = new CreatePostCommand(
                SharedMocks.MockedCurrentUserId,
                null,
                true,
                false,
                null,
                1);

            _postRepository.Setup(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsQuote(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsRepost(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                IsQUote = true,
                IsRepost = false,
                UserId = commandRequest.UserId
            });


            var commandResult = await commandHandler.Handle(commandRequest, new CancellationToken());

            Assert.IsNotNull(commandResult);
            Assert.IsNotNull(commandResult.Post);
            Assert.IsNull(commandResult.InvalidRequestMessage);
            _postRepository.Verify(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId), Times.Once);
            _postRepository.Verify(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(method => method.VerifyPostIsQuote(1), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsRepost(1), Times.Once);
        }

        [TestMethod]
        public async Task CantRepostOtherRepost()
        {
            var commandHandler = ObterHandler();
            var commandRequest = new CreatePostCommand(
                SharedMocks.MockedCurrentUserId,
                null,
                true,
                false,
                null,
                1);

            _postRepository.Setup(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsQuote(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsRepost(1)).ReturnsAsync(true);
            _postRepository.Setup(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                IsQUote = true,
                IsRepost = false,
                UserId = commandRequest.UserId
            });


            var commandResult = await commandHandler.Handle(commandRequest, new CancellationToken());

            Assert.IsNotNull(commandResult);
            Assert.IsNull(commandResult.Post);
            Assert.IsNotNull(commandResult.InvalidRequestMessage);
            _postRepository.Verify(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId), Times.Once);
            _postRepository.Verify(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsQuote(1), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsRepost(1), Times.Once);
        }

        [TestMethod]
        public async Task UserReachedPostLimit()
        {
            var commandHandler = ObterHandler();
            var commandRequest = new CreatePostCommand(
                SharedMocks.MockedCurrentUserId,
                null,
                true,
                false,
                null,
                1);

            _postRepository.Setup(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId)).ReturnsAsync(true);
            _postRepository.Setup(method => method.VerifyPostIsQuote(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.VerifyPostIsRepost(1)).ReturnsAsync(false);
            _postRepository.Setup(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                IsQUote = true,
                IsRepost = false,
                UserId = commandRequest.UserId
            });


            var commandResult = await commandHandler.Handle(commandRequest, new CancellationToken());

            Assert.IsNotNull(commandResult);
            Assert.IsNull(commandResult.Post);
            Assert.IsNotNull(commandResult.InvalidRequestMessage);
            _postRepository.Verify(method => method.VerifyUsersPostsToday(SharedMocks.MockedCurrentUserId), Times.Once);
            _postRepository.Verify(method => method.CreatePostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsQuote(1), Times.Never);
            _postRepository.Verify(method => method.VerifyPostIsRepost(1), Times.Never);
        }
        private CreatePostCommandHandler ObterHandler()
        {
            _postRepository = new Mock<IPostRepository>();
            return new CreatePostCommandHandler(_postRepository.Object);
        }
    }
}
