using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Services.Authenticates;
using Neon.Application.Services.Gameplays;
using Neon.Application.Services.Notifications;
using Neon.Application.Services.Systems;
using Neon.Application.Services.UserInputs;
using Neon.Application.Services.Users;

namespace Neon.Application;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddNeonApplication(this IServiceCollection services)
    {
        return services
            .AddSingleton<INotificationService, NotificationService>()
            .AddScoped<IAuthenticateService, AuthenticateService>()
            .AddScoped<IGameplayService, GameplayService>()
            .AddScoped<ISystemService, SystemService>()
            .AddScoped<IUserInputService, UserInputService>()
            .AddScoped<IUserService, UserService>();
    }
}