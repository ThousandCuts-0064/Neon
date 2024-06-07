using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Neon.Infrastructure;
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
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.HttpOnly = true;
        x.Cookie.SameSite = SameSiteMode.Strict;
        x.LoginPath = "/Authenticate";
        x.LogoutPath = x.LoginPath;
    });

builder.Services
    .AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services
    .AddNeonIdentity()
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();