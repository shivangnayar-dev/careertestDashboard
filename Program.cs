using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure authentication with cookie middleware
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/LoginBasic"; // Path to login page
        options.LogoutPath = "/Auth/LogoutBasic"; // Path to login page
        //options.AccessDeniedPath = "/Auth/AccessDenied"; // Path to access denied page
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<EmailService>();

//options =>
//{
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
//});

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddTransient<DatabaseRepositorycContext>();

builder.Services.AddDbContext<ApplicationDbContext> (options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)) // Adjust version based on your MySQL server version
    ));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//builder.Services.AddScoped<RoleManager<IdentityRole>>();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("SuperAdminPolicy", policy =>
//        policy.RequireRole("SuperAdmin"));
//    options.AddPolicy("AdminPolicy", policy =>
//        policy.RequireRole("Admin"));
//    options.AddPolicy("UserPolicy", policy =>
//        policy.RequireRole("User"));
//});

builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use authentication and authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=LoginBasic}/{id?}");

app.UseSession();

var scope = app.Services.CreateScope();
//var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

var roles = new[] { "SuperAdmin", "Admin", "User" };
foreach (var role in roles)
{
    //if (!await roleManager.RoleExistsAsync(role))
    //{
    //    await roleManager.CreateAsync(new IdentityRole(role));
    //}
}
app.Run();
