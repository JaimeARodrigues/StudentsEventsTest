using EventManager.Domain.Entities;
using EventManager.Domain.Interfaces;
using EventManager.Infrastructure.Persistence;

namespace EventManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EventManagerDbContext _context;

    public UserRepository(EventManagerDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await Task.FromResult(_context.Users.ToList());
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Id == id));
    }

    public async Task<User?> GetByGraphIdAsync(string graphId)
    {
        return await Task.FromResult(_context.Users.FirstOrDefault(u => u.GraphId == graphId));
    }

    public async Task<User?> GetWithEventsAsync(Guid id)
    {
        return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Id == id));
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
