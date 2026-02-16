using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventManager.Infrastructure.Persistence;

public class EventManagerDbContextFactory : IDesignTimeDbContextFactory<EventManagerDbContext>
{
    public EventManagerDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=EventManagerDb;Trusted_Connection=true;";

        var optionsBuilder = new DbContextOptionsBuilder<EventManagerDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new EventManagerDbContext(optionsBuilder.Options);
    }
}
