using EventManager.Domain.Entities;
using EventManager.Domain.Interfaces;
using EventManager.Infrastructure.Persistence;

namespace EventManager.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly EventManagerDbContext _context;

    public EventRepository(EventManagerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetByUserIdAsync(Guid userId)
    {
        return await Task.FromResult(_context.Events.Where(e => e.UserId == userId).ToList());
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(_context.Events.FirstOrDefault(e => e.Id == id));
    }

    public async Task<Event?> GetByGraphIdAsync(string graphId)
    {
        return await Task.FromResult(_context.Events.FirstOrDefault(e => e.GraphId == graphId));
    }

    public async Task AddAsync(Event @event)
    {
        await _context.Events.AddAsync(@event);
    }

    public async Task UpdateAsync(Event @event)
    {
        _context.Events.Update(@event);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var @event = await GetByIdAsync(id);
        if (@event != null)
        {
            _context.Events.Remove(@event);
        }
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var events = await GetByUserIdAsync(userId);
        _context.Events.RemoveRange(events);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
