namespace EventManager.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string GraphId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Subject { get; set; } = null!;
    public string? BodyPreview { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public string? Organizer { get; set; }
    public bool IsOnlineMeeting { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public User User { get; set; } = null!;
}
