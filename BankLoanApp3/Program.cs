//using BankLoanApp3.Data;
//using BankLoanApp3.Services;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//// Register the ApplicationDbContext with SQL Server
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Register your services
//builder.Services.AddScoped<HomeLoanService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=HomeLoan}/{action=Apply}/{id?}");

//app.Run();


using BankLoanApp3.Data;
using BankLoanApp3.Models;
using BankLoanApp3.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services and configure the ApplicationUser with Entity Framework Stores
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // Configure other identity options if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Register other services, such as HomeLoanService
builder.Services.AddScoped<HomeLoanService>();

// Add authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")); // Adjust to your roles or claims
});

// Add Controllers with Views
builder.Services.AddControllersWithViews();

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

// Authentication and Authorization middleware
app.UseAuthentication(); // This ensures the app uses the authentication system
app.UseAuthorization();  // This ensures the app uses authorization

app.UseEndpoints(endpoints =>
{
    // Route for HomeLoan controller
    endpoints.MapControllerRoute(
        name: "homeLoan",
        pattern: "HomeLoan/{action=Index}/{id?}",
        defaults: new { controller = "HomeLoan" });

    // Default route for other controllers
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed roles and admin user
        await DbInitializer.InitializeAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();


