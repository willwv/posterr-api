namespace Domain.Models
{
    public record UserMetadataDto {
        public UserMetadataDto(int postQuantity, string userName, DateTime dateJoined)
        {
            PostQuantity = postQuantity;
            UserName = userName;
            DateJoined = dateJoined.ToString("MMMM dd, yyyy");
        }
        public string UserName { get; set; }
        public string DateJoined { get; set; }
        public int PostQuantity { get; set; }
    }
}
