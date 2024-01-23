using Domain.Entities;
using MediatR;

namespace Application.Commands
{
    public record CreatePostCommand(int UserId, string? PostContent, bool IsRepost, bool IsQUote, string? Quote, int? OriginalPostId) : IRequest<Post>;

}
