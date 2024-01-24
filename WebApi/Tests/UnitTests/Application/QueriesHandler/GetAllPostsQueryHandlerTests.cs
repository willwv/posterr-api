using Application.Queries;
using Application.QueriesHandler;
using Domain.Interfaces.Repositories;
using Moq;
using Tests.UnitTests.Shared;

namespace Tests.UnitTests.Application.QueriesHandler
{
    [TestClass]
    public class GetAllPostsQueryHandlerTests
    {
        private Mock<IPostRepository> _postRepository;
        private GetAllPostsQueryHandler ObterHandler()
        {
            _postRepository = new Mock<IPostRepository>();
            return new GetAllPostsQueryHandler(_postRepository.Object);
        }

        [TestMethod]
        public async Task GetAllPosts()
        {
            var querydHandler = ObterHandler();
            var queryRequest = new GetAllPostsQuery(SharedMocks.MockedCurrentUserId, 0, 10, false, null);
            int skipItens = queryRequest.Page * queryRequest.ItensPerPage;
            _postRepository.Setup(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()))
                .ReturnsAsync(SharedMocks.GetMockedPostsList());

            var queryResul = await querydHandler.Handle(queryRequest, new CancellationToken());

            Assert.IsNotNull(queryResul);
            Assert.IsTrue(queryResul.Any());
            _postRepository.Verify(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllPosts_OutOfRange()
        {
            var querydHandler = ObterHandler();
            var queryRequest = new GetAllPostsQuery(SharedMocks.MockedCurrentUserId, 1, 10, false, null);
            int skipItens = queryRequest.Page * queryRequest.ItensPerPage;
            _postRepository.Setup(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()))
                .ReturnsAsync(SharedMocks.GetMockedPostsList(skip: skipItens));

            var queryResul = await querydHandler.Handle(queryRequest, new CancellationToken());

            Assert.IsNotNull(queryResul);
            Assert.IsFalse(queryResul.Any());
            _postRepository.Verify(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllPosts_FilterByOnlyMine()
        {
            var querydHandler = ObterHandler();
            var queryRequest = new GetAllPostsQuery(SharedMocks.MockedCurrentUserId, 0, 10, true, null);
            int skipItens = queryRequest.Page * queryRequest.ItensPerPage;
            _postRepository.Setup(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()))
                .ReturnsAsync(SharedMocks.GetMockedPostsList(userId: SharedMocks.MockedCurrentUserId));

            var queryResul = await querydHandler.Handle(queryRequest, new CancellationToken());

            Assert.IsNotNull(queryResul);
            Assert.IsTrue(queryResul.All(post => post.User.Id == SharedMocks.MockedCurrentUserId));
            _postRepository.Verify(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllPosts_FilterByDate()
        {
            var querydHandler = ObterHandler();
            var queryRequest = new GetAllPostsQuery(SharedMocks.MockedCurrentUserId, 0, 10, true, SharedMocks.MockedRandomDate);
            int skipItens = queryRequest.Page * queryRequest.ItensPerPage;
            _postRepository.Setup(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()))
                .ReturnsAsync(SharedMocks.GetMockedPostsList(createdAt: SharedMocks.MockedRandomDate));

            var queryResul = await querydHandler.Handle(queryRequest, new CancellationToken());

            Assert.IsNotNull(queryResul);
            Assert.IsTrue(queryResul.All(post => post.CreatedAt >= SharedMocks.MockedRandomDate));
            _postRepository.Verify(method => method.GetAllPostsAsync(queryRequest.UserId, queryRequest.OnlyMine, queryRequest.FromDate, skipItens, queryRequest.ItensPerPage, new CancellationToken()), Times.Once);
        }
    }
}
