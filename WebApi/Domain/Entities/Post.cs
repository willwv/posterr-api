namespace Domain.Entities
{
    public record Post
    {
        public Post(int userId, string content, DateTime createdAt)
        {
            UserId = userId;
            Content = content;
            CreatedAt = createdAt;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
