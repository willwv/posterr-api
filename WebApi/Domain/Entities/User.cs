using Domain.Models;

namespace Domain.Entities
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public IList<Post>? Posts { get; set; }

        public UserDto ToUserDto() => new(
        Id,
        UserName,
        CreatedAt);
    }
}
