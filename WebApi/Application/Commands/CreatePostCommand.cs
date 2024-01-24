using Domain.Entities;
using Domain.Models;
using MediatR;

namespace Application.Commands
{
    public record CreatePostCommand(int UserId, string? PostContent, bool IsRepost, bool IsQUote, string? Quote, int? OriginalPostId) : IRequest<CreatePostDto>;

}
