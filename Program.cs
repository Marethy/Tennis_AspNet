using Tennis.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Tennis.Interfaces;
using Tennis.Repositories;
using Tennis.Data;

var builder = WebApplication.CreateBuilder(args);

// ─── Services ─────────────────────────────────────────────────────────────────

// Controllers + Views + Razor
builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();

// EF Core
builder.Services.AddDbContext<TennisWebMVCContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// SignalR (only once)
builder.Services.AddSignalR();

// Repositories (scoped)
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IProductRatingRepository, ProductRatingRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Caching + Session (only one AddSession call)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Mail service
builder.Services.AddTransient<IMailService, MailService>();

// Authentication (two cookie schemes: “Signin” and “AdminSignin”)
builder.Services.AddAuthentication("Signin")
    .AddCookie("Signin", options =>
    {
        options.AccessDeniedPath = new PathString("/Account/Access");
        options.Cookie = new CookieBuilder
        {
            HttpOnly = true,
            Name = "SigninCookie",
            Path = "/",
            SameSite = SameSiteMode.Lax,
            SecurePolicy = CookieSecurePolicy.SameAsRequest
        };
        options.Events = new CookieAuthenticationEvents
        {
            OnSignedIn = ctx =>
            {
                Console.WriteLine($"{DateTime.Now} - OnSignedIn: {ctx.Principal.Identity.Name}");
                return Task.CompletedTask;
            },
            OnSigningOut = ctx =>
            {
                Console.WriteLine($"{DateTime.Now} - OnSigningOut: {ctx.HttpContext.User.Identity.Name}");
                return Task.CompletedTask;
            },
            OnValidatePrincipal = ctx =>
            {
                Console.WriteLine($"{DateTime.Now} - OnValidatePrincipal: {ctx.Principal.Identity.Name}");
                return Task.CompletedTask;
            }
        };
        options.LoginPath = new PathString("/User/Login");
        options.LogoutPath = "/User/Logout";
        options.ReturnUrlParameter = "ReturnUrl";
        options.SlidingExpiration = true;
    })
    .AddCookie("AdminSignin", options =>
    {
        options.AccessDeniedPath = new PathString("/Admin/AccessDenied");
        options.Cookie = new CookieBuilder
        {
            HttpOnly = true,
            Name = "AdminSigninCookie",
            Path = "/",
            SameSite = SameSiteMode.Lax,
            SecurePolicy = CookieSecurePolicy.SameAsRequest
        };
        options.Events = new CookieAuthenticationEvents
        {
            OnSignedIn = ctx =>
            {
                Console.WriteLine($"{DateTime.Now} - OnAdminSignedIn: {ctx.Principal.Identity.Name}");
                return Task.CompletedTask;
            },
            OnSigningOut = ctx =>
            {
                Console.WriteLine($"{DateTime.Now} - OnAdminSigningOut: {ctx.HttpContext.User.Identity.Name}");
                return Task.CompletedTask;
            },
            OnValidatePrincipal = ctx =>
            {
                Console.WriteLine($"{DateTime.Now} - OnAdminValidatePrincipal: {ctx.Principal.Identity.Name}");
                return Task.CompletedTask;
            }
        };
        options.LoginPath = new PathString("/Admin/AdmAccount/Login");
        options.LogoutPath = "/Admin/AdmAccount/Logout";
        options.ReturnUrlParameter = "ReturnUrl";
        options.SlidingExpiration = true;
    });

// (Optional) CORS policy for SignalR
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAllSignalR", policy =>
//     {
//         policy.AllowAnyHeader()
//               .AllowAnyMethod()
//               .SetIsOriginAllowed(_ => true)
//               .AllowCredentials();
//     });
// });

var app = builder.Build();

// ─── Middleware Pipeline ────────────────────────────────────────────────────────

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Session (only once)
app.UseSession();

// Authentication → Authorization (only once each)
app.UseAuthentication();
app.UseAuthorization();

// (Optional) Apply CORS before mapping Hubs
// app.UseCors("AllowAllSignalR");

// ─── Endpoints ──────────────────────────────────────────────────────────────────

// Area routes for Admin
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=AdmAccount}/{action=Login}/{id?}"
);

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ─── SIGNALR HUBS ──────────────────────────────────────────────────────────────
// Register each Hub exactly once
app.MapHub<CustomerHub>("/hubs/customerCount");
app.MapHub<AdminHub>("/hubs/adminHub");

app.Run();
