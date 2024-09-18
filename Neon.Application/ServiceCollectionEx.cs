using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Factories.Principals;
using Neon.Application.Services.Authentications;
using Neon.Application.Services.Items;
using Neon.Application.Services.Notifications;
using Neon.Application.Services.Passwords;
using Neon.Application.Services.Systems;
using Neon.Application.Services.UserInputs;
using Neon.Application.Services.UserRequests.Duel;
using Neon.Application.Services.UserRequests.Friend;
using Neon.Application.Services.UserRequests.Trade;
using Neon.Application.Services.Users;
using Neon.Application.Validators.Principals;
using Neon.Application.Validators.Users;

namespace Neon.Application;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddNeonApplication(this IServiceCollection services)
    {
        return services
            .AddSingleton<INotificationService, NotificationService>()
            .AddScoped<IPrincipalFactory, PrincipalFactory>()
            .AddScoped<IPrincipalValidator, PrincipalValidator>()
            .AddScoped<IUserValidator, UserValidator>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ISystemService, SystemService>()
            .AddScoped<IPasswordService, PasswordService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserInputService, UserInputService>()
            .AddScoped<IFriendRequestService, FriendRequestService>()
            .AddScoped<ITradeRequestService, TradeRequestService>()
            .AddScoped<IDuelRequestService, DuelRequestService>()
            .AddScoped<IItemService, ItemService>();
    }
}