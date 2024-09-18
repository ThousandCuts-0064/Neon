using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neon.Application;
using Neon.Application.Interfaces;
using Neon.Domain.Entities;
using Neon.Domain.Entities.UserRequests;
using Neon.Domain.Notifications.Bases;

namespace Neon.Data;

public class NeonDbContext : DbContext, INeonDbContext
{
    private readonly ILoggerFactory? _loggerFactory;
    private readonly string _connectionString;
    private readonly bool _isDevelopment;

    public required DbSet<SystemValue> SystemValues { get; init; }
    public required DbSet<User> Users { get; init; }
    public required DbSet<Friendship> Friendships { get; init; }
    public required DbSet<FriendRequest> FriendRequests { get; init; }
    public required DbSet<TradeRequest> TradeRequests { get; init; }
    public required DbSet<DuelRequest> DuelRequests { get; init; }

    public NeonDbContext() : this(FindAppSettings()) { }

    public NeonDbContext(
        IConfiguration configuration,
        ILoggerFactory? loggerFactory = null,
        IHostEnvironment? hostEnvironment = null)
    {
        _connectionString = configuration.GetConnectionString("Default") ??
            throw new ArgumentNullException(nameof(configuration), "No connection string with name \"Default\" found!");

        _loggerFactory = loggerFactory;
        _isDevelopment = hostEnvironment?.IsDevelopment() ?? true;
    }

    public async Task ListenAsync<T>() where T : Notification
    {
        #pragma warning disable EF1002
        await Database.ExecuteSqlRawAsync($"LISTEN \"{typeof(T).Name}\"");
        #pragma warning restore EF1002
    }

    public async Task NotifyAsync<T>(T payload) where T : Notification
    {
        await Database.ExecuteSqlAsync($"SELECT pg_notify({typeof(T).Name}, {JsonSerializer.Serialize(payload)})");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        optionsBuilder
            .UseNpgsql(_connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        if (!_isDevelopment)
            return;

        optionsBuilder
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NeonDbContext).Assembly);
    }

    private static IConfiguration FindAppSettings([CallerFilePath] string filePath = null!)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath("../../Neon.Web", filePath))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
}