using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Neon.Application;
using Neon.Domain.Entities;
using Neon.Domain.Entities.UserRequests;
using Neon.Domain.Notifications.Bases;

namespace Neon.Data;

public class NeonDbContext : DbContext, INeonDbContext
{
    private const string SCHEMA = "public";

    private const string CONNECTION_STRING =
        $"""
         User Id=postgres;
         Password=pgsql;
         Host=localhost;
         Database=neon;
         Port=5432;
         Persist Security Info=true;
         Search Path={SCHEMA};
         """;

    private readonly string? _connectionString;

    public DbSet<SystemValue> SystemValues { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Friendship> Friendships { get; init; }
    public DbSet<FriendRequest> FriendRequests { get; init; }
    public DbSet<TradeRequest> TradeRequests { get; init; }
    public DbSet<DuelRequest> DuelRequests { get; init; }

    public NeonDbContext() => _connectionString = CONNECTION_STRING;

    public NeonDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    #pragma warning disable EF1002
    public async Task ListenAsync<T>() where T : Notification
    {
        await Database.ExecuteSqlRawAsync($"LISTEN \"{typeof(T).Name}\"");
    }

    public async Task NotifyAsync<T>(T payload) where T : Notification
    {
        await Database.ExecuteSqlRawAsync($"NOTIFY \"{typeof(T).Name}\", {JsonSerializer.Serialize(payload)}");
    }
    #pragma warning restore EF1002

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString, x =>
            {
                x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, SCHEMA);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NeonDbContext).Assembly);
    }
}