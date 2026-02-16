namespace EventManager.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = null!;
    public string? BodyPreview { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public string? Organizer { get; set; }
    public bool IsOnlineMeeting { get; set; }
}
