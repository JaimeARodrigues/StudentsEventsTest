namespace EventManager.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string GraphId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? GivenName { get; set; }
    public string? Surname { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
