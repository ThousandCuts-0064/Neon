using Microsoft.EntityFrameworkCore;
using Neon.Application.Services.UserRequests.Bases;
using Neon.Application.Services.Users;
using Neon.Domain.Entities.UserRequests;
using Neon.Domain.Notifications;

namespace Neon.Application.Services.UserRequests.Duel;

internal class DuelRequestService :
    UserRequestService<
        DuelRequest,
        DuelRequestSent, DuelRequestAccepted, DuelRequestDeclined, DuelRequestCanceled>,
    IDuelRequestService
{
    protected override DbSet<DuelRequest> DbSet => DbContext.DuelRequests;

    public DuelRequestService(INeonDbContext dbContext, IUserService userService) : base(dbContext, userService) { }
}