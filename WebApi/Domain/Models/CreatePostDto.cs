using Domain.Entities;

namespace Domain.Models
{
    public record CreatePostDto
    {
        public Post? Post {  get; set; }
        public IList<string> InvalidRequestMessage { get; set; }
    }
}
