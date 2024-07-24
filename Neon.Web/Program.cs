using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.DbNotifications;
using Neon.Domain.Entities;
using Neon.Domain.Enums;
using Neon.Infrastructure;
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

builder.Services.AddSignalR();

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
        x.MinificationSettings.RemoveTagsWithoutContent = true;
        x.MinificationSettings.WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive;
    });

builder.Services
    .AddDbContext<NeonDbContext>()
    .AddNeonInfrastructure(builder.Configuration)
    .AddIdentity<User, IdentityRole<int>>(x =>
    {
        x.User.AllowedUserNameCharacters = new string(Enumerable
            .Range('a', 'z' - 'a')
            .Concat(Enumerable.Range('A', 'Z' - 'A'))
            .Concat(Enumerable.Range('0', '9' - '0'))
            .Select(y => (char)y)
            .Concat([' ', '_', '-', '.', '@', '+'])
            .ToArray());

        x.Password.RequiredLength = User.PASSWORD_MIN_LENGTH;
        x.Password.RequireLowercase = true;
        x.Password.RequireUppercase = true;
        x.Password.RequireDigit = true;
    })
    .AddEntityFrameworkStores<NeonDbContext>();

builder.Services.AddAntiforgery();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie.HttpOnly = true;
    x.Cookie.SameSite = SameSiteMode.Strict;
    x.Cookie.IsEssential = true;
    x.LoginPath = "/Authenticate";
    x.LogoutPath = x.LoginPath;
});

builder.Services
    .AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole(Enum.GetNames<UserRole>())
        .Build());

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

app.MapHub<GameplayHub>("/Gameplay/Hub");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

await app.UseNeonInfrastructure(x =>
{
    var dbNotificationService = x.GetRequiredService<IDbNotificationService>();
    var gameplayHubContex = x.GetRequiredService<IHubContext<GameplayHub, IGameplayHubClient>>();

    dbNotificationService.Listen<ActiveConnectionToggle>(y =>
        gameplayHubContex.Clients.All.ActiveConnectionToggle(y));

    return ValueTask.CompletedTask;
});

await app.RunAsync();