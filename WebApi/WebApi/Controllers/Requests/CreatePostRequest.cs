namespace WebApi.Controllers.Requests
{
     public record CreatePostRequest(string? PostContent, bool IsRepost, bool IsQUote, string? Quote, int? OriginalPostId);
}
