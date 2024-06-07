using Microsoft.EntityFrameworkCore;
using Neon.Domain.Users;

namespace Neon.Data.Core;

public class NeonDbContext : DbContext
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

    public DbSet<User> Users { get; init; } = null!;

    public NeonDbContext() : this(CONNECTION_STRING) { }
    public NeonDbContext(string? connectionString) => _connectionString = connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString, x =>
            {
                x.MigrationsHistoryTable("__EFMigrationsHistory", SCHEMA);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NeonDbContext).Assembly);
    }
}