using Domain.Models;
using MediatR;

namespace Application.Queries
{
    public record GetUserMetadataQuery(int UserId) : IRequest<UserMetadataDto?>;
}

