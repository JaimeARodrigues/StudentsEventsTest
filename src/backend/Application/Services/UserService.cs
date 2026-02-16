using EventManager.Application.DTOs;
using EventManager.Domain.Entities;
using EventManager.Domain.Interfaces;

namespace EventManager.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            DisplayName = u.DisplayName,
            GivenName = u.GivenName,
            Surname = u.Surname
        }).ToList();
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            GivenName = user.GivenName,
            Surname = user.Surname
        };
    }

    public async Task<(UserDto? User, List<EventDto> Events)?> GetUserWithEventsAsync(Guid id)
    {
        var user = await _userRepository.GetWithEventsAsync(id);
        if (user == null) return null;

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            GivenName = user.GivenName,
            Surname = user.Surname
        };

        var eventsDto = user.Events.Select(e => new EventDto
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

        return (userDto, eventsDto);
    }
}
