using Microsoft.EntityFrameworkCore;
using Neon.Application.Services.UserRequests.Bases;
using Neon.Application.Services.Users;
using Neon.Domain.Entities.UserRequests;
using Neon.Domain.Notifications;

namespace Neon.Application.Services.UserRequests.Trade;

internal class TradeRequestService :
    UserRequestService<
        TradeRequest,
        TradeRequestSent, TradeRequestAccepted, TradeRequestDeclined, TradeRequestCanceled>,
    ITradeRequestService
{
    protected override DbSet<TradeRequest> DbSet => DbContext.TradeRequests;

    public TradeRequestService(INeonDbContext dbContext, IUserService userService) : base(dbContext, userService) { }
}