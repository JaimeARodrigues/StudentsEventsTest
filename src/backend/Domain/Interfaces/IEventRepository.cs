using EventManager.Domain.Entities;

namespace EventManager.Domain.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetByUserIdAsync(Guid userId);
    Task<Event?> GetByIdAsync(Guid id);
    Task<Event?> GetByGraphIdAsync(string graphId);
    Task AddAsync(Event @event);
    Task UpdateAsync(Event @event);
    Task DeleteAsync(Guid id);
    Task DeleteByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
}
