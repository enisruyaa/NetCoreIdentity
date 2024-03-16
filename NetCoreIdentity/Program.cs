using NetCoreIdentity.Models.ContextClasses;
using Microsoft.EntityFrameworkCore;
using NetCoreIdentity.Models.Entities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequiredLength = 3;
    x.Password.RequireDigit = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Lockout.MaxFailedAccessAttempts = 5;
    x.User.RequireUniqueEmail = true; // Bir e-mail sadece bir kullan�c�da olabilsin diye bunu yazmam�z gerkiyor eskiden otomatik olarak true geliyordu fakat son g�ncelleme ile birlikte bunu kendi elimiz ile birlikte true yapmam�z gerekiyor yoksa birden �ok kullan�c� ayn� mail edresi ile kay�t olabilir.
}).AddEntityFrameworkStores<MyContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    x.Cookie.Name = "CyberSelf";
    x.ExpireTimeSpan = TimeSpan.FromDays(1);
    x.Cookie.SameSite = SameSiteMode.Strict;
    x.LoginPath = new PathString("/Home/SignIn");
    x.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

builder.Services.AddDbContextPool<MyContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());
 
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Authentication (Kimlik Tan�mas�) olmadan Authorization (Yetki) olmaz.

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
