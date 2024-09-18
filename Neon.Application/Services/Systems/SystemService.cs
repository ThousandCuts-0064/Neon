using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Interfaces;
using Neon.Application.Services.Bases;
using Neon.Domain.Entities;

namespace Neon.Application.Services.Systems;

internal class SystemService : DbContextService, ISystemService
{
    private const string LAST_ACTIVE_AT = "LastActiveAt";
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.ffffffzzz";

    public SystemService(INeonDbContext dbContext) : base(dbContext) { }

    public async Task<DateTime?> FindLastActiveAtAsync()
    {
        var lastActiveAt = await DbContext.SystemValues
            .Where(x => x.Id == LAST_ACTIVE_AT)
            .Select(x => x.Value)
            .FirstOrDefaultAsync();

        if (lastActiveAt is null)
            return null;

        return DateTime.ParseExact(
            lastActiveAt,
            DATE_TIME_FORMAT,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);
    }

    public async Task UpsertLastActiveAtAsync()
    {
        var dateTimeString = DateTime.Now.ToString(DATE_TIME_FORMAT, CultureInfo.InvariantCulture);

        var updatedCount = await DbContext.SystemValues
            .Where(x => x.Id == LAST_ACTIVE_AT)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Value, dateTimeString));

        if (updatedCount > 0)
            return;

        DbContext.SystemValues.Add(new SystemValue
        {
            Id = LAST_ACTIVE_AT,
            Value = dateTimeString
        });

        await DbContext.SaveChangesAsync();
    }
}