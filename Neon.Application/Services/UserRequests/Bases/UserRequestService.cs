using Microsoft.EntityFrameworkCore;
using Neon.Application.Services.Bases;
using Neon.Domain.Entities.UserRequests.Bases;
using Neon.Domain.Notifications;
using Neon.Domain.Notifications.Bases;

namespace Neon.Application.Services.UserRequests.Bases;

internal abstract class UserRequestService<
    TUserRequest,
    TUserRequestSent, TUserRequestAccepted, TUserRequestDeclined, TUserRequestCanceled> :
    DbContextService,
    IUserRequestService
    where TUserRequest : class, IUserRequest, new()
    where TUserRequestSent : Notification, IUserRequestSent, new()
    where TUserRequestAccepted : Notification, IUserRequestAccepted, new()
    where TUserRequestDeclined : Notification, IUserRequestDeclined, new()
    where TUserRequestCanceled : Notification, IUserRequestCanceled, new()
{
    protected abstract DbSet<TUserRequest> DbSet { get; }

    protected UserRequestService(INeonDbContext dbContext) : base(dbContext) { }

    public async Task SendAsync(int requesterId, int responderId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        DbSet.Add(new TUserRequest
        {
            RequesterId = requesterId,
            ResponderId = responderId
        });

        await DbContext.SaveChangesAsync();

        var requester = await FindRequesterAsync(requesterId);

        await DbContext.NotifyAsync(new TUserRequestSent
        {
            RequesterId = requesterId,
            RequesterKey = requester.Key,
            RequesterUsername = requester.Username,
            ResponderId = responderId
        });

        await transaction.CommitAsync();
    }

    public async Task AcceptAsync(int requesterId, int responderId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var requester = await FindRequesterAsync(requesterId);

        await DbContext.NotifyAsync(new TUserRequestAccepted
        {
            RequesterId = requesterId,
            RequesterKey = requester.Key,
            RequesterUsername = requester.Username,
            ResponderId = responderId
        });

        await DbSet
            .Where(x => x.ResponderId == requesterId && x.ResponderId == requesterId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }

    public async Task DeclineAsync(int requesterId, int responderId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var requester = await FindRequesterAsync(requesterId);

        await DbContext.NotifyAsync(new TUserRequestDeclined
        {
            RequesterId = requesterId,
            RequesterKey = requester.Key,
            RequesterUsername = requester.Username,
            ResponderId = responderId
        });

        await DbSet
            .Where(x => x.ResponderId == requesterId && x.ResponderId == requesterId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }

    public async Task CancelAsync(int requesterId, int responderId)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var requester = await FindRequesterAsync(requesterId);

        await DbContext.NotifyAsync(new TUserRequestCanceled
        {
            RequesterId = requesterId,
            RequesterKey = requester.Key,
            RequesterUsername = requester.Username,
            ResponderId = responderId
        });

        await DbSet
            .Where(x => x.ResponderId == requesterId && x.ResponderId == requesterId)
            .ExecuteDeleteAsync();

        await transaction.CommitAsync();
    }

    private Task<Requester> FindRequesterAsync(int id)
    {
        return DbContext.Users
            .Where(x => x.Id == id)
            .Select(x => new Requester
            {
                Key = x.Key,
                Username = x.Username
            })
            .FirstAsync();
    }

    private class Requester
    {
        public required Guid Key { get; init; }
        public required string Username { get; init; }
    }
}