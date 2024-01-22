using Application.Queries;
using Domain.Interfaces.Repositories;
using Domain.Models;
using MediatR;

namespace Application.QueriesHandler
{
    public class GetUserMetadataQueryHandler : IRequestHandler<GetUserMetadataQuery, UserMetadataDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserMetadataQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserMetadataDto> Handle(GetUserMetadataQuery request, CancellationToken cancellationToken)
        {
            var userMetadata = await _userRepository.GetUserMetadata(request.UserId, cancellationToken);
            return userMetadata;
        }
    }
}
