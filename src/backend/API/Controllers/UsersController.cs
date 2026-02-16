using Microsoft.AspNetCore.Mvc;
using EventManager.Application.DTOs;
using EventManager.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace EventManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("{id}/events")]
    public async Task<ActionResult> GetUserWithEvents(Guid id)
    {
        var result = await _userService.GetUserWithEventsAsync(id);
        if (result == null)
            return NotFound();

        return Ok(new
        {
            user = result.Value.User,
            events = result.Value.Events
        });
    }
}
