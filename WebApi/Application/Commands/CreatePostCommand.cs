using Domain.Entities;
using MediatR;

namespace Application.Commands
{
    public record CreatePostCommand(int UserId, string PostContent) : IRequest<Post>;

}
