namespace Domain.Models
{
    public record PostDto(
        int Id,
        string? Content,
        DateTime CreatedAt,
        UserDto User,
        bool IsRepost,
        bool IsQuote,
        string? Quote,
        int? OriginalPostId);
}
