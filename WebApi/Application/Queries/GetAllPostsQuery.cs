using Domain.Entities;
using MediatR;

namespace Application.Queries
{
    public record GetAllPostsQuery(int UserId, int Page, int ItensPerPage, bool OnlyMine, DateTime? FromDate) : IRequest<IList<Post>>;
}
