<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authentication.Cookies;
=======
using Tennis.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
>>>>>>> be7f762a1570641dbd99902d4ae2fd2c7eb5be01
using Microsoft.EntityFrameworkCore;
using Tennis.Hubs;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Repositories;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// DB
builder.Services.AddDbContext<TennisWebMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// View & Razor
=======
// Add services to the container.
>>>>>>> be7f762a1570641dbd99902d4ae2fd2c7eb5be01
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TennisWebMVCContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
<<<<<<< HEAD
builder.Services.AddSignalR(); // Add SignalR services

// Repositories
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
=======
>>>>>>> be7f762a1570641dbd99902d4ae2fd2c7eb5be01
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
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
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian session tồn tại
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// 💡 Authentication
builder.Services.AddAuthentication("Signin") // <- scheme name
    .AddCookie("Signin", options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
    });

builder.Services.AddAuthorization(); // không có gì đặc biệt nếu bạn dùng mặc định



<<<<<<< HEAD
// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
=======
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

        // Set login paths for different user types
        options.LoginPath = new PathString("/User/Login");  // For regular users
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

        // Set login path for admin
        options.LoginPath = new PathString("/Admin/AdmAccount/Login"); // For admin users
        options.LogoutPath = "/Admin/AdmAccount/Logout";
        options.ReturnUrlParameter = "ReturnUrl";
        options.SlidingExpiration = true;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var emailConfig = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
//builder.Services.AddSingleton(emailConfig);
builder.Services.AddControllers();

// signal R count number customer online
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins()
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

//var vnPaySettings = builder.Configuration.GetSection("VnPaySettings").Get<VnPaySettings>();
//builder.Services.AddSingleton(vnPaySettings);
var payOSSettings = builder.Configuration.GetSection("PayOSSettings").Get<PayOSProperties>();
//builder.Services.AddSingleton(payOSSettings);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
>>>>>>> be7f762a1570641dbd99902d4ae2fd2c7eb5be01
    app.UseHsts();
}
app.MapHub<AdminHub>("/hubs/adminHub");  // Map the hub endpoint


// handler error 404 page
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode = {0}");


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
// 💡 Auth middlewares
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthentication();

app.UseSession();

// belong to signal R
app.UseCors();


//app.MapControllerRoute(
//    name: "Admin",
//    pattern: "{area:exists}/{controller=AdmBlog}/{action=Index}/{id?}");


app.MapAreaControllerRoute(
    "Admin",
    "Admin",
    "Admin/{controller=AdmAccount}/{action=Login}/{id?}");

app.MapControllerRoute(
<<<<<<< HEAD
    name: "default",
    // pattern: "{controller=User}/{action=Register}/{id?}");
     pattern: "{controller=Home}/{action=Index}/{id?}");
=======
    "default",
    "{controller=Home}/{action=Index}/{id?}");
>>>>>>> be7f762a1570641dbd99902d4ae2fd2c7eb5be01

    //pattern: "{area=Admin}/{controller=AdmOrder}/{action=Index}/{id?}");

app.MapHub<CustomerHub>("/hubs/customerCount");
app.MapHub<AdminHub>("/hubs/adminHub");

app.Run();