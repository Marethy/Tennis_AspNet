using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Tennis.Hubs;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<TennisWebMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Razor & View
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// SignalR
builder.Services.AddSignalR();

// Repositories
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

// Mail
builder.Services.AddTransient<IMailService, MailService>();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication
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
            OnSignedIn = context =>
            {
                Console.WriteLine("{0} - {1}: {2}", DateTime.Now, "OnSignedIn", context.Principal.Identity.Name);
                return Task.CompletedTask;
            },
            OnSigningOut = context =>
            {
                Console.WriteLine("{0} - {1}: {2}", DateTime.Now, "OnSigningOut", context.HttpContext.User.Identity.Name);
                return Task.CompletedTask;
            },
            OnValidatePrincipal = context =>
            {
                Console.WriteLine("{0} - {1}: {2}", DateTime.Now, "OnValidatePrincipal", context.Principal.Identity.Name);
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
            OnSignedIn = context =>
            {
                Console.WriteLine("{0} - {1}: {2}", DateTime.Now, "OnAdminSignedIn", context.Principal.Identity.Name);
                return Task.CompletedTask;
            },
            OnSigningOut = context =>
            {
                Console.WriteLine("{0} - {1}: {2}", DateTime.Now, "OnAdminSigningOut", context.HttpContext.User.Identity.Name);
                return Task.CompletedTask;
            },
            OnValidatePrincipal = context =>
            {
                Console.WriteLine("{0} - {1}: {2}", DateTime.Now, "OnAdminValidatePrincipal", context.Principal.Identity.Name);
                return Task.CompletedTask;
            }
        };
        options.LoginPath = new PathString("/Admin/AdmAccount/Login");
        options.LogoutPath = "/Admin/AdmAccount/Logout";
        options.ReturnUrlParameter = "ReturnUrl";
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// CORS (nếu cần thiết)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Optional: load config settings
var emailConfig = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
var payOSSettings = builder.Configuration.GetSection("PayOSSettings").Get<PayOSProperties>();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapAreaControllerRoute(
    "Admin",
    "Admin",
    "Admin/{controller=AdmAccount}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR Hubs
app.MapHub<CustomerHub>("/hubs/customerCount");
app.MapHub<AdminHub>("/hubs/adminHub");

app.Run();
