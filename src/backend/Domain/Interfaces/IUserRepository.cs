using EventManager.Domain.Entities;

namespace EventManager.Domain.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByGraphIdAsync(string graphId);
    Task<User?> GetWithEventsAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
