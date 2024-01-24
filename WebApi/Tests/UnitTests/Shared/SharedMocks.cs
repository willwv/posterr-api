using Domain.Entities;

namespace Tests.UnitTests.Shared
{
    public static class SharedMocks
    {
        public static DateTime MockedRandomDate = DateTime.UtcNow.AddDays(-5);
        public const int MockedCurrentUserId = 1;
        
        public static List<Post> GetMockedPostsList(int? userId = null, DateTime? createdAt = null, int skip = 0, int take = 10)
        {
            var posts = new List<Post>()
            {
                new() {
                    Id = 1,
                    UserId = MockedCurrentUserId,
                    CreatedAt = MockedRandomDate,
                    Content = "It's a mocked Post number 1.",
                    IsQUote = false,
                    IsRepost = false,
                    Quote = null,
                    OriginalPostId = 1,
                },
                new() {
                    Id = 2,
                    UserId = 2,
                    CreatedAt = DateTime.UtcNow,
                    Content = null,
                    IsQUote = true,
                    IsRepost = false,
                    Quote = "It's a mocked Quote por post number 1",
                    OriginalPostId = 1,
                }
            };

            if(userId != null )
            {
                posts = posts.Where(post => post.UserId == userId).ToList();
            }

            if(createdAt != null)
            {
                posts = posts.Where(post => post.CreatedAt == createdAt).ToList();
            }

            return posts.Skip(skip).Take(take).ToList();
        }
    }
}
