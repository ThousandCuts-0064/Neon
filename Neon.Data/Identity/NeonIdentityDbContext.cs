using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Neon.Data.Identity;

public class NeonIdentityDbContext : IdentityDbContext<NeonUser, IdentityRole<int>, int>
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

    public const string SCHEMA = "identity";

    private readonly string? _connectionString;

    public NeonIdentityDbContext() : this(CONNECTION_STRING) { }
    public NeonIdentityDbContext(string? connectionString) => _connectionString = connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString, x =>
            {
                x.MigrationsHistoryTable("__EFMigrationsHistory", SCHEMA);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var neonUser = builder.Entity<NeonUser>();

        neonUser.Property(x => x.RegisteredAt).IsRequired();
        neonUser.Property(x => x.LastActiveAt).IsRequired();
    }
}