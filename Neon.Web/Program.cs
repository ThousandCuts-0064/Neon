using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Neon.Data;
using Neon.Domain.Users;
using Neon.Infrastructure;
using Neon.Web.Hubs;
using Neon.Web.Resources;
using Neon.Web.Utils.Extensions;
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
    .AddDbContext<NeonDbContext>((x, _) => x
        .GetRequiredService<IConfiguration>()
        .GetConnectionString("Default"))
    .AddNeonInfrastructure()
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
app.UseAuthentication();
app.UseAuthorization();

await app.EnsureRoles(Enum.GetNames<UserRole>());

app.MapHub<GameplayHub>("Gameplay/Hub");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();