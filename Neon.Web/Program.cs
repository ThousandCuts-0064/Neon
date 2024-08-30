using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Neon.Application;
using Neon.Application.Services.Notifications;
using Neon.Application.Validators.Principals;
using Neon.Domain.Enums;
using Neon.Domain.Notifications;
using Neon.Domain.Notifications.Bases;
using Neon.Infrastructure;
using Neon.Web.Args.Client;
using Neon.Web.Hubs;
using Neon.Web.Resources;
using Neon.Web.Utils.Localization;
using WebMarkupMin.AspNetCore8;
using WebMarkupMin.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddMvcOptions(x =>
    {
        x.ModelBindingMessageProvider.CopyFrom<LocalizedBindingMessageProvider>();
        x.ModelMetadataDetailsProviders.Add(new LocalizedMetadataDetailsProvider());
    })
    .AddDataAnnotationsLocalization(x =>
    {
        x.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(Resource));
    });

builder.Services.AddSignalR(x =>
{
    if (builder.Environment.IsDevelopment())
        x.EnableDetailedErrors = true;
});

builder.Services
    .AddWebMarkupMin(x =>
    {
        x.AllowMinificationInDevelopmentEnvironment = true;
        x.AllowCompressionInDevelopmentEnvironment = true;
    })
    .AddHtmlMinification(x =>
    {
        x.MinificationSettings.EmptyTagRenderMode = HtmlEmptyTagRenderMode.NoSlash;
        x.MinificationSettings.RemoveOptionalEndTags = true;
        x.MinificationSettings.CollapseBooleanAttributes = true;
        x.MinificationSettings.AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5;
        x.MinificationSettings.RemoveRedundantAttributes = true;
        x.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
        x.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
        x.MinificationSettings.RemoveEmptyAttributes = true;
        //x.MinificationSettings.RemoveTagsWithoutContent = true;
        x.MinificationSettings.WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive;
    });

builder.Services.AddAntiforgery();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.HttpOnly = true;
        x.Cookie.SameSite = SameSiteMode.Strict;
        x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        x.Cookie.IsEssential = true;
        x.LoginPath = "/Authenticate";
        x.LogoutPath = x.LoginPath;

        x.Events.OnValidatePrincipal = y => y.HttpContext.RequestServices
            .GetRequiredService<IPrincipalValidator>()
            .ValidateAsync(y);
    })
    .AddIdentityCookies();

builder.Services
    .AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole(Enum.GetNames<UserRole>())
        .Build());

builder.Services
    .AddHttpContextAccessor()
    .AddNeonInfrastructure(builder.Configuration)
    .AddNeonApplication();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseWebMarkupMin();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();

app.MapHub<LobbyHub>("/Lobby/Hub");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{key?}");

await app.UseNeonInfrastructureAsync(x =>
{
    var notificationService = x.GetRequiredService<INotificationService>();
    var lobbyHubContex = x.GetRequiredService<IHubContext<LobbyHub, ILobbyClient>>();

    notificationService.Listen<UserConnectionToggled>(y =>
    {
        lobbyHubContex.Clients.All.UserConnectionToggled(new UserConnectionToggledArgs
        {
            Username = y.Username,
            IsActive = y.IsActive
        });
    });

    ForwardToClient<FriendRequestSent, FriendRequestSentArgs>(y => y.FriendRequestSent);
    ForwardToClient<FriendRequestAccepted, FriendRequestAcceptedArgs>(y => y.FriendRequestAccepted);
    ForwardToClient<FriendRequestDeclined, FriendRequestDeclinedArgs>(y => y.FriendRequestDeclined);
    ForwardToClient<FriendRequestCanceled, FriendRequestCanceledArgs>(y => y.FriendRequestCanceled);

    ForwardToClient<TradeRequestSent, TradeRequestSentArgs>(y => y.TradeRequestSent);
    ForwardToClient<TradeRequestAccepted, TradeRequestAcceptedArgs>(y => y.TradeRequestAccepted);
    ForwardToClient<TradeRequestDeclined, TradeRequestDeclinedArgs>(y => y.TradeRequestDeclined);
    ForwardToClient<TradeRequestCanceled, TradeRequestCanceledArgs>(y => y.TradeRequestCanceled);

    ForwardToClient<DuelRequestSent, DuelRequestSentArgs>(y => y.DuelRequestSent);
    ForwardToClient<DuelRequestAccepted, DuelRequestAcceptedArgs>(y => y.DuelRequestAccepted);
    ForwardToClient<DuelRequestDeclined, DuelRequestDeclinedArgs>(y => y.DuelRequestDeclined);
    ForwardToClient<DuelRequestCanceled, DuelRequestCanceledArgs>(y => y.DuelRequestCanceled);

    return ValueTask.CompletedTask;

    void ForwardToClient<TUserRequest, TServerUserRequestArgs>(
        Func<ILobbyClient, Func<TServerUserRequestArgs, Task>> methodSelector)
        where TUserRequest : Notification, IUserRequestNotification
        where TServerUserRequestArgs : IServerUserRequestArgs, new()
    {
        notificationService.Listen<TUserRequest>(y =>
        {
            var client = lobbyHubContex.Clients.User(y.ResponderId.ToString());

            methodSelector(client)(new TServerUserRequestArgs
            {
                RequesterKey = y.RequesterKey,
                RequesterUsername = y.RequesterUsername
            });
        });
    }
});

await app.RunAsync();