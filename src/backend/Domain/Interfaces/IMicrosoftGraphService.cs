namespace EventManager.Domain.Interfaces;

public interface IMicrosoftGraphService
{
    Task<List<(string Id, string Email, string DisplayName, string? GivenName, string? Surname)>> GetUsersAsync();
    Task<List<(string Id, string Subject, string? BodyPreview, DateTime StartTime, DateTime EndTime, string? Location, string? Organizer, bool IsOnlineMeeting)>> GetUserEventsAsync(string userEmail);
}
