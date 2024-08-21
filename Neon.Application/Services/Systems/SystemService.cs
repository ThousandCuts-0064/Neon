using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Neon.Domain.Entities;

namespace Neon.Application.Services.Systems;

internal class SystemService : ISystemService
{
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.ffffffzzz";

    private readonly INeonDbContext _dbContext;

    public SystemService(INeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DateTime> GetLastActiveAtAsync()
    {
        var lastActiveAt = await _dbContext.SystemValues
            .Where(x => x.Id == SystemValue.LAST_ACTIVE_AT)
            .Select(x => x.Value)
            .FirstAsync();

        return DateTime.ParseExact(
            lastActiveAt,
            DATE_TIME_FORMAT,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);
    }

    public async Task UpsertLastActiveAtAsync()
    {
        var dateTimeString = DateTime.Now.ToString(DATE_TIME_FORMAT, CultureInfo.InvariantCulture);

        var updatedCount = await _dbContext.SystemValues
            .Where(x => x.Id == SystemValue.LAST_ACTIVE_AT)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Value, dateTimeString));

        if (updatedCount > 0)
            return;

        _dbContext.SystemValues.Add(new SystemValue
        {
            Id = SystemValue.LAST_ACTIVE_AT,
            Value = dateTimeString
        });
    }
}