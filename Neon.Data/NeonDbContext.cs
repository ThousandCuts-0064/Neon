using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Neon.Domain.Users;

namespace Neon.Data;

public class NeonDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    private const string CONNECTION_STRING =
        "User Id=postgre;" +
        "Password=pgsql;" +
        "Host=127.0.0.1;" +
        "Database=neon;" +
        "Port=5432;" +
        "Persist Security Info=true;" +
        "Initial Schema=" + SCHEMA + ";" +
        "Unicode=true";

    public const string SCHEMA = "public";

    public DbSet<User> Users { get; init; } = null!;

    public NeonDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_configuration.GetConnectionString("Default"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}