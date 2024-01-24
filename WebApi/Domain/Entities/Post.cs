using Domain.Models;

namespace Domain.Entities
{
    public class Post
    {
        public Post() { }
        public Post(int userId, string content)
        {
            UserId = userId;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }

        public Post(int userId, int? originalPostId, bool isRepost)
        {
            UserId = userId;
            OriginalPostId = originalPostId;
            IsRepost = isRepost;
            CreatedAt = DateTime.UtcNow;
        }
        public Post(int userId, int? originalPostId, bool isQUote, string? quote)
        {
            UserId = userId;
            OriginalPostId = originalPostId;
            IsQUote = isQUote;
            Quote = quote;
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRepost { get; set; }
        public bool IsQUote { get; set; }
        public string? Quote { get; set; }
        public int? OriginalPostId { get; set; }

        public PostDto ToPostDto() => new(
                Id,
                Content,
                CreatedAt,
                User!.ToUserDto(),
                IsRepost,
                IsQUote,
                Quote,
                OriginalPostId);
    }
}
