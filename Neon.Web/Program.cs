using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Neon.Application;
using Neon.Application.Validators.Principals;
using Neon.Domain.Enums;
using Neon.Infrastructure;
using Neon.Web;
using Neon.Web.Hubs;
using Neon.Web.Resources;
using Neon.Web.Utils.Localization;
using WebMarkupMin.AspNetCore8;
using WebMarkupMin.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews(x =>
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

builder.Host.UseNeonInfrastructure();

var app = builder.Build();

app.UseNeonInfrastructure();

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

await app.InitializeNeonAsync<Initializer>();

await app.RunAsync();