using Neon.Application;
using Neon.Data;
using Neon.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.MaxValue;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<INeonApplication>(x =>
    new NeonApplication(new NeonDomain(new NeonDbContext(x.GetRequiredService<IConfiguration>()))));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();