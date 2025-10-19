// Program.cs
// Entry point for the NexaCart ASP.NET Core application.
// Configures services, middleware pipeline, and starts the web server.

using EcomRazorPage.Models;
using EcomRazorPage.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure ASP.NET Core Identity with custom ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings for enhanced security
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings for brute force protection
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Sign-in settings
    options.SignIn.RequireConfirmedEmail = false; // Set to true for production
})
.AddEntityFrameworkStores<ArticlesDBContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// Configure cookie settings for security
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Add Razor Pages services
builder.Services.AddRazorPages();

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ArticlesDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session services for shopping cart functionality
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add logging services for better error tracking
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
});

// Add HTTP client for external API calls (if needed)
builder.Services.AddHttpClient();

// Add memory cache for performance optimization
builder.Services.AddMemoryCache();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
// Use different error handling in development vs production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // In development, use detailed error pages
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint(); // Enable EF migrations endpoint in development
}

// Security middleware - order matters
app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles();      // Serve static files (CSS, JS, images)

app.UseRouting();          // Enable routing

// Authentication and Authorization middleware
app.UseAuthentication();   // Must come before UseAuthorization
app.UseAuthorization();

// Enable session middleware
app.UseSession();

// Redirect root URL to public product listings
app.MapGet("/", context => {
    context.Response.Redirect("/Public");
    return Task.CompletedTask;
});

// Map Razor Pages endpoints
app.MapRazorPages();

// Map ASP.NET Core Identity UI (login, register, etc.)
app.MapRazorPages();

// Ensure database is created and migrated on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ArticlesDBContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Apply any pending migrations
        context.Database.Migrate();

        // Seed initial data if needed
        await DbInitializer.InitializeAsync(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Start the web application
app.Run();
