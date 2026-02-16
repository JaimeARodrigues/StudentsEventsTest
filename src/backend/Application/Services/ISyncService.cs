using System.Threading.Tasks;

namespace EventManager.Application.Services;

public interface ISyncService
{
    Task SyncUsersAsync();
    Task SyncEventsAsync();
}
