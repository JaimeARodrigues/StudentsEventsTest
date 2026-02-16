using EventManager.Domain.Interfaces;

namespace EventManager.Application.Services;

public class SyncService : ISyncService
{
    private readonly MicrosoftGraphService _graphService;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<SyncService> _logger;

    public SyncService(MicrosoftGraphService graphService, IUserRepository userRepository, IEventRepository eventRepository, ILogger<SyncService> logger)
    {
        _graphService = graphService;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task SyncUsersAsync()
    {
        var graphUsers = await _graphService.GetUsersAsync();

        foreach (var graphUser in graphUsers)
        {
            var existingUser = await _userRepository.GetByGraphIdAsync(graphUser.Id);

            if (existingUser == null)
            {
                var newUser = new Domain.Entities.User
                {
                    Id = Guid.NewGuid(),
                    GraphId = graphUser.Id,
                    Email = graphUser.Email,
                    DisplayName = graphUser.DisplayName,
                    GivenName = graphUser.GivenName,
                    Surname = graphUser.Surname,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _userRepository.AddAsync(newUser);
            }
            else
            {
                existingUser.Email = graphUser.Email;
                existingUser.DisplayName = graphUser.DisplayName;
                existingUser.GivenName = graphUser.GivenName;
                existingUser.Surname = graphUser.Surname;
                existingUser.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(existingUser);
            }
        }

        await _userRepository.SaveChangesAsync();
        _logger.LogInformation("Successfully synced {Count} users", graphUsers.Count);
    }

    public async Task SyncEventsAsync()
    {
        var users = await _userRepository.GetAllAsync();
        int totalEvents = 0;

        foreach (var user in users)
        {
            try
            {
                var graphEvents = await _graphService.GetUserEventsAsync(user.Email);

                await _eventRepository.DeleteByUserIdAsync(user.Id);

                foreach (var graphEvent in graphEvents)
                {
                    var newEvent = new Domain.Entities.Event
                    {
                        Id = Guid.NewGuid(),
                        GraphId = graphEvent.Id,
                        UserId = user.Id,
                        Subject = graphEvent.Subject,
                        BodyPreview = graphEvent.BodyPreview,
                        StartTime = graphEvent.StartTime,
                        EndTime = graphEvent.EndTime,
                        Location = graphEvent.Location,
                        Organizer = graphEvent.Organizer,
                        IsOnlineMeeting = graphEvent.IsOnlineMeeting,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _eventRepository.AddAsync(newEvent);
                    totalEvents++;
                }

                await _eventRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing events for user {Email}", user.Email);
            }
        }

        _logger.LogInformation("Successfully synced {Count} events", totalEvents);
    }
}
