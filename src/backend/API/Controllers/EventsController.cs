using Microsoft.AspNetCore.Mvc;
using EventManager.Application.DTOs;
using EventManager.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace EventManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly EventService _eventService;

    public EventsController(EventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<EventDto>>> GetByUserId(Guid userId)
    {
        var events = await _eventService.GetEventsByUserIdAsync(userId);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(Guid id)
    {
        var @event = await _eventService.GetEventByIdAsync(id);
        if (@event == null)
            return NotFound();

        return Ok(@event);
    }
}
