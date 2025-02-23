using EmailApp.Data;
using EmailApp.Data.Repositories;
using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.IEntities;
using EmailApp.Services;
using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
builder.Services.AddControllersWithViews();

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnString")));

builder.Services.Configure<SMTPCredentials>(builder.Configuration.GetSection("SMTPCredentials"));


// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ISubscribedRepository, SubscribedRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ISubscribedService, SubscribedService>();

// Configure authentication using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";   // Redirect if not authenticated
        options.LogoutPath = "/Auth/Logout"; // Redirect after logout
        options.AccessDeniedPath = "/Home/AccessDenied"; // Optional: Handle access denied cases
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Authentication must be added before Authorization
app.UseAuthentication();
app.UseAuthorization();

// ✅ Ensure static assets work correctly
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}") // Redirect users to login first
    .WithStaticAssets();

app.Run();
