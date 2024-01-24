using Application.Queries;
using Application.QueriesHandler;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Moq;
using Tests.UnitTests.Shared;

namespace Tests.UnitTests.Application.QueriesHandler
{
    [TestClass]
    public class GetUserMetadataQueryHandlerTests
    {
        private Mock<IUserRepository> _userRepository;
        private GetUserMetadataQueryHandler ObterHandler()
        {
            _userRepository = new Mock<IUserRepository>();
            return new GetUserMetadataQueryHandler(_userRepository.Object);
        }

        [TestMethod]
        public async Task GetUserMetadata()
        {
            var querydHandler = ObterHandler();
            var queryRequest = new GetUserMetadataQuery(SharedMocks.MockedCurrentUserId);

            _userRepository.Setup(method => method.GetUserMetadata(queryRequest.UserId, new CancellationToken()))
                .ReturnsAsync(new UserMetadataDto(1, "Mocekd User", DateTime.UtcNow));

            var queryResul = await querydHandler.Handle(queryRequest, new CancellationToken());

            Assert.IsNotNull(queryResul);
            _userRepository.Verify(method => method.GetUserMetadata(queryRequest.UserId, new CancellationToken()), Times.Once);
        }
    }
}
