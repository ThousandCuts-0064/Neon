using Microsoft.EntityFrameworkCore;
using Neon.Domain.Users;

namespace Neon.Data;

public class NeonDbContext : DbContext
{
    private readonly string? _connectionString;

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

    public DbSet<User> Users { get; init; } = null!;

    public NeonDbContext() : this(CONNECTION_STRING) { }
    public NeonDbContext(string? connectionString) => _connectionString = connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NeonDbContext).Assembly);
    }
}