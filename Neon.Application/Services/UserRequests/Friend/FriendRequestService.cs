using Microsoft.EntityFrameworkCore;
using Neon.Application.Services.UserRequests.Bases;
using Neon.Domain.Entities;
using Neon.Domain.Notifications;

namespace Neon.Application.Services.UserRequests.Friend;

internal class FriendRequestService :
    UserRequestService<
        FriendRequest,
        FriendRequestSent, FriendRequestAccepted, FriendRequestDeclined, FriendRequestCanceled>,
    IFriendRequestService
{
    protected override DbSet<FriendRequest> DbSet => DbContext.FriendRequests;

    public FriendRequestService(INeonDbContext dbContext) : base(dbContext) { }
}