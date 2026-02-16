using Microsoft.AspNetCore.Mvc;
using EventManager.Domain.Interfaces;
using EventManager.Application.DTOs;

namespace EventManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;

    public DiagnosticsController(IUserRepository userRepository, IEventRepository eventRepository)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
    }

    [HttpGet("db-stats")]
    public async Task<IActionResult> GetDbStats()
    {
        var users = await _userRepository.GetAllAsync();
        int usersCount = users.Count;

        int totalEvents = 0;
        UserDto? firstUserWithEvents = null;
        List<EventDto>? firstUserEvents = null;

        foreach (var user in users)
        {
            var events = await _eventRepository.GetByUserIdAsync(user.Id);
            if (events != null && events.Count > 0)
            {
                if (firstUserWithEvents == null)
                {
                    firstUserWithEvents = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        GivenName = user.GivenName,
                        Surname = user.Surname
                    };

                    firstUserEvents = events.Select(e => new EventDto
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

                totalEvents += events.Count;
            }
        }

        return Ok(new
        {
            UsersCount = usersCount,
            EventsCount = totalEvents,
            FirstUserWithEvents = firstUserWithEvents,
            FirstUserEvents = firstUserEvents
        });
    }
}
