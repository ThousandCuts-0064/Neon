using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Neon.Application;
using Neon.Domain.Entities;
using Neon.Domain.Notifications.Bases;

namespace Neon.Data;

public class NeonDbContext : IdentityDbContext<User, IdentityRole<int>, int>, INeonDbContext
{
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

    public const string SCHEMA = "public";

    private readonly string? _connectionString;

    public DbSet<SystemValue> SystemValues { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<TradeRequest> TradeRequests { get; set; }
    public DbSet<DuelRequest> DuelRequests { get; set; }

    public NeonDbContext() => _connectionString = CONNECTION_STRING;

    public NeonDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public async Task ListenAsync<T>() where T : Notification
    {
        await Database.ExecuteSqlAsync($"LISTEN \"{nameof(T)}\"");
    }

    public async Task NotifyAsync<T>(T payload) where T : Notification
    {
        await Database.ExecuteSqlAsync($"NOTIFY \"{nameof(T)}\", {JsonSerializer.Serialize(payload)}");
    }

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NeonDbContext).Assembly);
    }
}