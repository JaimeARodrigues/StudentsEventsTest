using EventManager.Application.DTOs;
using EventManager.Domain.Interfaces;

namespace EventManager.Application.Services;

public class EventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<List<EventDto>> GetEventsByUserIdAsync(Guid userId)
    {
        var events = await _eventRepository.GetByUserIdAsync(userId);
        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Subject = e.Subject,
            BodyPreview = e.BodyPreview,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Location = e.Location,
            Organizer = e.Organizer,
            IsOnlineMeeting = e.IsOnlineMeeting
        }).ToList();
    }

    public async Task<EventDto?> GetEventByIdAsync(Guid id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event == null) return null;

        return new EventDto
        {
            Id = @event.Id,
            Subject = @event.Subject,
            BodyPreview = @event.BodyPreview,
            StartTime = @event.StartTime,
            EndTime = @event.EndTime,
            Location = @event.Location,
            Organizer = @event.Organizer,
            IsOnlineMeeting = @event.IsOnlineMeeting
        };
    }
}
