namespace Neon.Application.Services;

public interface ISystemValueService
{
    Task<DateTime> GetLastActiveAtAsync();
    Task UpsertLastActiveAtAsync();
}