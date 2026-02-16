using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.Persistence;

public class EventManagerDbContext : DbContext
{
    public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GraphId).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired();
            entity.HasIndex(e => e.GraphId).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasMany(e => e.Events).WithOne(e => e.User).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GraphId).IsRequired();
            entity.Property(e => e.Subject).IsRequired();
            entity.HasIndex(e => e.GraphId).IsUnique();
            entity.HasIndex(e => e.UserId);
        });
    }
}
