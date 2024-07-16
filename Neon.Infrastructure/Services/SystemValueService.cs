using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Repositories;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.Entities;
using Neon.Infrastructure.Services.Bases;

namespace Neon.Infrastructure.Services;

internal class SystemValueService : DbContextService, ISystemValueService
{
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.ffffffzzz";

    private readonly ISystemValueRepository _systemValueRepository;

    public SystemValueService(NeonDbContext dbContext, ISystemValueRepository systemValueRepository) : base(dbContext)
    {
        _systemValueRepository = systemValueRepository;
    }

    public async Task<DateTime> GetLastActiveAtAsync()
    {
        var lastActiveAt = await _systemValueRepository.FindRequiredByIdAsync(SystemValue.LAST_ACTIVE_AT);

        return DateTime.ParseExact(
            lastActiveAt.Value,
            DATE_TIME_FORMAT,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);
    }

    public async Task UpsertLastActiveAtAsync()
    {
        var dateTimeString = DateTime.Now.ToString(DATE_TIME_FORMAT, CultureInfo.InvariantCulture);

        var updatedCount = await _systemValueRepository
            .FilterById(SystemValue.LAST_ACTIVE_AT)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Value, dateTimeString));

        if (updatedCount > 0)
            return;

        _systemValueRepository.Create(new SystemValue
        {
            Id = SystemValue.LAST_ACTIVE_AT,
            Value = dateTimeString
        });

        await DbContext.SaveChangesAsync();
    }
}