namespace Domain.Entities
{
    public record User
    {
        public User() { }
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public IList<Post>? Posts { get; set; }
    }
}
