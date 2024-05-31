using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Neon.Infrastructure;
using Neon.Web.Resources;
using Neon.Web.Utils.Localization;

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

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.MaxValue;
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

builder.Services.AddNeonApplication();

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseSession();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();