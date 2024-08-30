namespace Neon.Application.Services.Systems;

public interface ISystemService
{
    Task<DateTime?> FindLastActiveAtAsync();
    Task UpsertLastActiveAtAsync();
}