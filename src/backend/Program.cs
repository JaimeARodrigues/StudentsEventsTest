using EventManager.Application.Services;
using EventManager.Domain.Interfaces;
using EventManager.Infrastructure.Persistence;
using EventManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Add DbContext
builder.Services.AddDbContext<EventManagerDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

// Add Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<MicrosoftGraphService>();
// Register sync service and interface for Hangfire background jobs
builder.Services.AddScoped<ISyncService, SyncService>();

// Add JWT Authentication
var jwtSecret = configuration["Jwt:Secret"] ?? "your-super-secret-key-here-change-in-production";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };
        policyBuilder
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add Hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        })
);

builder.Services.AddHangfireServer();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Manager API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Add Logging
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Migrate Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EventManagerDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Manager API v1"));

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Hangfire Dashboard
app.UseHangfireDashboard();

app.Run();
