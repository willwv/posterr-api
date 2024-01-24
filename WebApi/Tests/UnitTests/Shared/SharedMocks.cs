using Domain.Entities;
using Domain.Models;

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
                    User = new()
                    {
                        Id = MockedCurrentUserId,
                        UserName = "Mocked User",
                        CreatedAt = DateTime.UtcNow
                    },
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
                    User = new()
                    {
                        Id = 2,
                        UserName = "Mocked User 2",
                        CreatedAt = DateTime.UtcNow
                    },
                    CreatedAt = DateTime.UtcNow,
                    Content = null,
                    IsQUote = true,
                    IsRepost = false,
                    Quote = "It's a mocked Quote por post number 1",
                    OriginalPostId = 1,
                }
            };

            if (userId != null)
            {
                posts = posts.Where(post => post.UserId == userId).ToList();
            }

            if (createdAt != null)
            {
                posts = posts.Where(post => post.CreatedAt == createdAt).ToList();
            }

            return posts.Skip(skip).Take(take).ToList();
        }

        public static List<PostDto> GetMockedPostsDtoList(int? userId = null, DateTime? createdAt = null, int skip = 0, int take = 10)
        {
            var posts = new List<PostDto>()
            {
                new(
                    1,
                    "It's a mocked Post number 1.",
                    MockedRandomDate,
                    new UserDto(MockedCurrentUserId, "Mocked User", DateTime.UtcNow),
                    false,
                    false,
                    null,
                    null
                ),
                new(
                    2,
                    "It's a mocked Post number 2.",
                    DateTime.UtcNow,
                    new UserDto(2, "Mocked User", DateTime.UtcNow),
                    false,
                    false,
                    null,
                    null)
            };

            if (userId != null)
            {
                posts = posts.Where(post => post.User.Id == userId).ToList();
            }

            if (createdAt != null)
            {
                posts = posts.Where(post => post.CreatedAt == createdAt).ToList();
            }

            return posts.Skip(skip).Take(take).ToList();
        }
    }
}
