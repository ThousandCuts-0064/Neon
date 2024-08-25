﻿using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Services.Bases;
using Neon.Domain.Entities;

namespace Neon.Application.Services.Systems;

internal class SystemService : DbContextService, ISystemService
{
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.ffffffzzz";

    public SystemService(INeonDbContext dbContext) : base(dbContext) { }

    public async Task<DateTime> GetLastActiveAtAsync()
    {
        var lastActiveAt = await DbContext.SystemValues
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

        var updatedCount = await DbContext.SystemValues
            .Where(x => x.Id == SystemValue.LAST_ACTIVE_AT)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Value, dateTimeString));

        if (updatedCount > 0)
            return;

        DbContext.SystemValues.Add(new SystemValue
        {
            Id = SystemValue.LAST_ACTIVE_AT,
            Value = dateTimeString
        });

        await DbContext.SaveChangesAsync();
    }
}