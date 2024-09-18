using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Interfaces;
using Neon.Application.Services.Bases;
using Neon.Application.Services.Users;
using Neon.Domain.Entities.UserRequests.Bases;
using Neon.Domain.Notifications;

namespace Neon.Application.Services.UserRequests.Bases;

internal abstract class UserRequestService<
    TUserRequest,
    TUserRequestSent, TUserRequestAccepted, TUserRequestDeclined, TUserRequestCanceled> :
    DbContextService,
    IUserRequestService
    where TUserRequest : UserRequest, new()
    where TUserRequestSent : UserRequestSent, new()
    where TUserRequestAccepted : UserRequestAccepted, new()
    where TUserRequestDeclined : UserRequestDeclined, new()
    where TUserRequestCanceled : UserRequestCanceled, new()
{
    private readonly IUserService _userService;
    protected abstract DbSet<TUserRequest> DbSet { get; }

    protected UserRequestService(INeonDbContext dbContext, IUserService userService) : base(dbContext)
    {
        _userService = userService;
    }

    public async Task SendAsync(int requesterId, Guid requesterKey, string requesterUsername, Guid responderKey)
    {
        if (requesterKey == responderKey)
            return;

        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var responderId = await _userService.FindIdAsync(responderKey);

        var existingRequesterId = await DbSet
            .Where(x =>
                x.RequesterId == requesterId && x.ResponderId == responderId ||
                x.RequesterId == responderId && x.ResponderId == requesterId)
            .Select(x => x.RequesterId)
            .FirstOrDefaultAsync();

        if (existingRequesterId == 0)
        {
            DbSet.Add(new TUserRequest
            {
                RequesterId = requesterId,
                ResponderId = responderId,
                CreatedAt = DateTime.UtcNow
            });

            await DbContext.SaveChangesAsync();

            await DbContext.NotifyAsync(new TUserRequestSent
            {
                RequesterId = requesterId,
                RequesterKey = requesterKey,
                RequesterUsername = requesterUsername,
                ResponderId = responderId,
                ResponderKey = responderKey
            });

            await transaction.CommitAsync();

            return;
        }

        if (existingRequesterId == requesterId)
            return;

        var responderUsername = await _userService.FindUsername(responderId);

        // The responder accepts because they have sent a request too.
        await DbContext.NotifyAsync(new TUserRequestAccepted
        {
            RequesterId = requesterId,
            RequesterKey = requesterKey,
            ResponderId = responderId,
            ResponderKey = responderKey,
            ResponderUsername = responderUsername
        });

        // The requester becomes a responder and accepts.
        await DbContext.NotifyAsync(new TUserRequestAccepted
        {
            RequesterId = responderId,
            RequesterKey = responderKey,
            ResponderId = requesterId,
            ResponderKey = requesterKey,
            ResponderUsername = requesterUsername
        });

        // Remove the existing request that is with reversed requester and responder.
        await DbSet
            .Where(x => x.RequesterId == responderId && x.ResponderId == requesterId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }

    public async Task AcceptAsync(Guid requesterKey, int responderId, Guid responderKey, string responderUsername)
    {
        if (requesterKey == responderKey)
            return;

        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var requesterId = await _userService.FindIdAsync(requesterKey);

        var deletedCount = await DbSet
            .Where(x => x.RequesterId == requesterId && x.ResponderId == responderId)
            .ExecuteDeleteAsync();

        if (deletedCount > 0)
        {
            await DbContext.NotifyAsync(new TUserRequestAccepted
            {
                RequesterId = requesterId,
                RequesterKey = requesterKey,
                ResponderId = responderId,
                ResponderKey = responderKey,
                ResponderUsername = responderUsername
            });
        }

        await transaction.CommitAsync();
    }

    public async Task DeclineAsync(Guid requesterKey, int responderId, Guid responderKey, string responderUsername)
    {
        if (requesterKey == responderKey)
            return;

        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var requesterId = await _userService.FindIdAsync(requesterKey);

        var deletedCount = await DbSet
            .Where(x => x.RequesterId == requesterId && x.ResponderId == responderId)
            .ExecuteDeleteAsync();

        if (deletedCount > 0)
        {
            await DbContext.NotifyAsync(new TUserRequestDeclined
            {
                RequesterId = requesterId,
                RequesterKey = requesterKey,
                ResponderId = responderId,
                ResponderKey = responderKey,
                ResponderUsername = responderUsername
            });
        }

        await transaction.CommitAsync();
    }

    public async Task CancelAsync(int requesterId, Guid requesterKey, string requesterUsername, Guid responderKey)
    {
        if (requesterKey == responderKey)
            return;

        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var responderId = await _userService.FindIdAsync(responderKey);

        var deletedCount = await DbSet
            .Where(x => x.RequesterId == requesterId && x.ResponderId == responderId)
            .ExecuteDeleteAsync();

        if (deletedCount > 0)
        {
            await DbContext.NotifyAsync(new TUserRequestCanceled
            {
                RequesterId = requesterId,
                RequesterKey = requesterKey,
                RequesterUsername = requesterUsername,
                ResponderId = responderId,
                ResponderKey = responderKey
            });
        }

        await transaction.CommitAsync();
    }
}