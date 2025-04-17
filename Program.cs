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

// View & Razor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSignalR(); // Add SignalR services

// Repositories
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
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

// 💡 Authentication
builder.Services.AddAuthentication("Signin") // <- scheme name
    .AddCookie("Signin", options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
    });

builder.Services.AddAuthorization(); // không có gì đặc biệt nếu bạn dùng mặc định


var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.MapHub<AdminHub>("/hubs/adminHub");  // Map the hub endpoint


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 💡 Auth middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
     pattern: "{controller=User}/{action=Register}/{id?}");
  //  pattern: "{area=Admin}/{controller=AdmOrder}/{action=Index}/{id?}");

app.Run();
