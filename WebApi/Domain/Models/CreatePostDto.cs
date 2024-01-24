using Domain.Entities;

namespace Domain.Models
{
    public record CreatePostDto
    {
        public PostDto? Post { get; set; }
        public IList<string> InvalidRequestMessage { get; set; }
    }
}
