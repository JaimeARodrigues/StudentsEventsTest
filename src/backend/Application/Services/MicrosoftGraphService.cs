using EventManager.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventManager.Application.Services;

public class MicrosoftGraphService : IMicrosoftGraphService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MicrosoftGraphService> _logger;

    public MicrosoftGraphService(IConfiguration configuration, ILogger<MicrosoftGraphService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<(string Id, string Email, string DisplayName, string? GivenName, string? Surname)>> GetUsersAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "https://graph.microsoft.com/v1.0/users?$select=id,mail,displayName,givenName,surname&$top=999";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var users = new List<(string Id, string Email, string DisplayName, string? GivenName, string? Surname)>();

            if (doc.RootElement.TryGetProperty("value", out var items) && items.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in items.EnumerateArray())
                {
                    var id = item.GetPropertyOrDefault("id");
                    var mail = item.GetPropertyOrDefault("mail");
                    var displayName = item.GetPropertyOrDefault("displayName");
                    var givenName = item.GetNullableProperty("givenName");
                    var surname = item.GetNullableProperty("surname");

                    users.Add((id, mail, displayName, givenName, surname));
                }
            }

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users from Microsoft Graph");
            throw;
        }
    }

    public async Task<List<(string Id, string Subject, string? BodyPreview, DateTime StartTime, DateTime EndTime, string? Location, string? Organizer, bool IsOnlineMeeting)>> GetUserEventsAsync(string userEmail)
    {
        try
        {
            var token = await GetAccessTokenAsync();
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var start = DateTime.UtcNow.AddDays(-30).ToString("O");
            var end = DateTime.UtcNow.AddDays(365).ToString("O");

            var url = $"https://graph.microsoft.com/v1.0/users/{Uri.EscapeDataString(userEmail)}/calendarView?startDateTime={Uri.EscapeDataString(start)}&endDateTime={Uri.EscapeDataString(end)}&$select=id,subject,bodyPreview,start,end,location,organizer,isOnlineMeeting&$top=999";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var events = new List<(string Id, string Subject, string? BodyPreview, DateTime StartTime, DateTime EndTime, string? Location, string? Organizer, bool IsOnlineMeeting)>();

            if (doc.RootElement.TryGetProperty("value", out var items) && items.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in items.EnumerateArray())
                {
                    var id = item.GetPropertyOrDefault("id");
                    var subject = item.GetPropertyOrDefault("subject");
                    var bodyPreview = item.GetNullableProperty("bodyPreview");

                    DateTime startTime = ParseGraphDateTime(item, "start");
                    DateTime endTime = ParseGraphDateTime(item, "end");

                    var location = item.TryGetProperty("location", out var loc) && loc.ValueKind == JsonValueKind.Object
                        ? (loc.TryGetProperty("displayName", out var dn) ? dn.GetString() : null)
                        : null;

                    var organizer = item.TryGetProperty("organizer", out var org) && org.ValueKind == JsonValueKind.Object
                        ? (org.TryGetProperty("emailAddress", out var ea) && ea.TryGetProperty("name", out var name) ? name.GetString() : null)
                        : null;

                    var isOnline = item.TryGetProperty("isOnlineMeeting", out var iom) && iom.ValueKind == JsonValueKind.True;

                    events.Add((id, subject, bodyPreview, startTime, endTime, location, organizer, isOnline));
                }
            }

            return events;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching events from Microsoft Graph for user {UserEmail}", userEmail);
            throw;
        }
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var clientId = _configuration["AzureAd:ClientId"];
        var clientSecret = _configuration["AzureAd:ClientSecret"];
        var tenantId = _configuration["AzureAd:TenantId"];

        var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
        using var httpClient = new HttpClient();

        var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "scope", "https://graph.microsoft.com/.default" }
        };

        var content = new FormUrlEncodedContent(requestBody);
        var response = await httpClient.PostAsync(tokenUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Failed to acquire token from Azure AD");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var token = JsonDocument.Parse(jsonResponse)
            .RootElement.GetProperty("access_token").GetString();

        return token ?? string.Empty;
    }

    private static DateTime ParseGraphDateTime(JsonElement item, string propertyName)
    {
        try
        {
            if (item.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Object)
            {
                if (prop.TryGetProperty("dateTime", out var dt) && dt.ValueKind == JsonValueKind.String && dt.GetString() is string s)
                {
                    if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var parsed))
                        return parsed;
                    return DateTime.Parse(s);
                }
                // Fallback: try direct string
                if (prop.ValueKind == JsonValueKind.String && prop.GetString() is string ds)
                {
                    return DateTime.Parse(ds);
                }
            }
        }
        catch
        {
            // ignore and fall through
        }

        return DateTime.UtcNow;
    }
}

internal static class JsonExtensions
{
    public static string GetPropertyOrDefault(this JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String)
            return prop.GetString() ?? string.Empty;
        return string.Empty;
    }

    public static string? GetNullableProperty(this JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String)
            return prop.GetString();
        return null;
    }
}
