using Microsoft.EntityFrameworkCore;
using Neon.Application.Services.Bases;
using Neon.Domain.Entities.Bases;
using Neon.Domain.Notifications;
using Neon.Domain.Notifications.Bases;

namespace Neon.Application.Services.UserRequests.Bases;

internal abstract class UserRequestService<
    TUserRequest,
    TUserRequestSent, TUserRequestAccepted, TUserRequestDeclined, TUserRequestCanceled> :
    DbContextService, IUserRequestService
    where TUserRequest : class, IUserRequest, new()
    where TUserRequestSent : Notification, IUserRequestSent, new()
    where TUserRequestAccepted : Notification, IUserRequestAccepted, new()
    where TUserRequestDeclined : Notification, IUserRequestDeclined, new()
    where TUserRequestCanceled : Notification, IUserRequestCanceled, new()
{
    protected abstract DbSet<TUserRequest> DbSet { get; }

    protected UserRequestService(INeonDbContext dbContext) : base(dbContext) { }

    public async Task SendAsync(int requesterUserId, int responderUserId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        DbSet.Add(new TUserRequest
        {
            RequesterUserId = requesterUserId,
            ResponderUserId = responderUserId
        });

        await DbContext.SaveChangesAsync();

        await DbContext.NotifyAsync(new TUserRequestSent
        {
            RequesterUserId = requesterUserId,
            ResponderUserId = responderUserId
        });

        await transaction.CommitAsync();
    }

    public async Task AcceptAsync(int requesterUserId, int responderUserId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        await DbContext.NotifyAsync(new TUserRequestAccepted
        {
            RequesterUserId = requesterUserId,
            ResponderUserId = responderUserId
        });

        await DbSet
            .Where(x => x.ResponderUserId == requesterUserId && x.ResponderUserId == requesterUserId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }

    public async Task DeclineAsync(int requesterUserId, int responderUserId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        await DbContext.NotifyAsync(new TUserRequestDeclined
        {
            RequesterUserId = requesterUserId,
            ResponderUserId = responderUserId
        });

        await DbSet
            .Where(x => x.ResponderUserId == requesterUserId && x.ResponderUserId == requesterUserId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }

    public async Task CancelAsync(int requesterUserId, int responderUserId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        await DbContext.NotifyAsync(new TUserRequestCanceled
        {
            RequesterUserId = requesterUserId,
            ResponderUserId = responderUserId
        });

        await DbSet
            .Where(x => x.ResponderUserId == requesterUserId && x.ResponderUserId == requesterUserId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }
}