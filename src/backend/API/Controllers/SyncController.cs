using Microsoft.AspNetCore.Mvc;
using Hangfire;
using EventManager.Application.Services;
using EventManager.Domain.Interfaces;

namespace EventManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly ILogger<SyncController> _logger;

    public SyncController(ILogger<SyncController> logger)
    {
        _logger = logger;
    }

    [HttpPost("users")]
    public IActionResult SyncUsers()
    {
        // Queue a background job to perform the actual work so the HTTP request returns immediately
        BackgroundJob.Enqueue<ISyncService>(s => s.SyncUsersAsync());
        _logger.LogInformation("Enqueued background job to sync users");
        return Accepted(new { message = "User sync enqueued" });
    }

    [HttpPost("events")]
    public IActionResult SyncEvents()
    {
        // Queue a background job to perform the heavy events sync
        BackgroundJob.Enqueue<ISyncService>(s => s.SyncEventsAsync());
        _logger.LogInformation("Enqueued background job to sync events");
        return Accepted(new { message = "Events sync enqueued" });
    }

    [HttpPost("schedule")]
    public IActionResult ScheduleSyncJob()
    {
        RecurringJob.AddOrUpdate<ISyncService>(
            "sync-users",
            s => s.SyncUsersAsync(),
            Cron.Hourly);

        RecurringJob.AddOrUpdate<ISyncService>(
            "sync-events",
            s => s.SyncEventsAsync(),
            Cron.Hourly);

        return Ok(new { message = "Sync jobs scheduled" });
    }
}
