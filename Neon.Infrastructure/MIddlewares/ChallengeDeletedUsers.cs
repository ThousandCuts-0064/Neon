using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Neon.Data;

namespace Neon.Infrastructure.MIddlewares;

internal class ChallengeDeletedUsers : IMiddleware
{
    private readonly NeonDbContext _dbContext;

    public ChallengeDeletedUsers(NeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) &&
            await _dbContext.Users.AllAsync(x => x.Id != userId))
        {
            context.Response.Cookies.Delete(".AspNetCore.Identity.Application");

            await context.ChallengeAsync();

            return;
        }

        await next(context);
    }
}