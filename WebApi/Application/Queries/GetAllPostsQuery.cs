using Domain.Entities;
using MediatR;

namespace Application.Queries
{
    public record GetAllPostsQuery : IRequest<List<Post>>;
}
