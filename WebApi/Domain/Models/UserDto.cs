namespace Domain.Models
{
    public record UserDto(
        int Id,
        string UserName,
        DateTime CreatedAt);
}
