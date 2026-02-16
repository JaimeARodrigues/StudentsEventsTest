namespace EventManager.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? GivenName { get; set; }
    public string? Surname { get; set; }
}
