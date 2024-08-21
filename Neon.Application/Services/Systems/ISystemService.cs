namespace Neon.Application.Services.Systems;

public interface ISystemService
{
    Task<DateTime> GetLastActiveAtAsync();
    Task UpsertLastActiveAtAsync();
}