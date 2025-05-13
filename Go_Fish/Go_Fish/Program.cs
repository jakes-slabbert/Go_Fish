
using GoFishData;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GoFishHelpers.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Events;

using Microsoft.AspNetCore.Identity.UI.Services;
using GoFishData.Entities;
using DevExpress.XtraCharts;
using GoFish.Services.CurrentUser;
using MediatR;
using System.Reflection;
using GoFish.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddAntiforgery(s =>
{
    s.HeaderName = "X-CSRF-TOKEN";
    s.Cookie.Name = "GoFish.Token";
    s.Cookie.SameSite = SameSiteMode.Strict;
    s.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    s.Cookie.HttpOnly = true;
    //s.SuppressXFrameOptionsHeader = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<IdentityOptions>(config =>
{
    config.User.RequireUniqueEmail = true;
    config.Password.RequiredLength = 6;
    config.Password.RequireUppercase = false;
    config.Password.RequireLowercase = false;
    config.Password.RequireNonAlphanumeric = false;
    config.SignIn.RequireConfirmedEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "GoFish";
    options.ExpireTimeSpan = TimeSpan.FromDays(15);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LogoutPath = "/Identity/Account/Logout";
    options.SlidingExpiration = true;

    options.Events.OnRedirectToLogin = context =>
    {
        var returnUrl = context.Request.Path.Value;
        if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/")
        {
            returnUrl = "/Asset/Index"; // Default redirect
        }
        context.Response.Redirect(returnUrl);
        return Task.CompletedTask;
    };
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 443;
});

// ----------- Custom Services -----------
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<ICurrencyHelper, CurrencyHelper>();
// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddProblemDetails();
builder.Services.AddRazorPages().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    options.JsonSerializerOptions.WriteIndented = true;
});

// For Identity  
builder.Services.AddIdentity<AppUser, AppRole>()
        .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

// Adding Authentication  

builder.Services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(c =>
                {
                    c.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:Schemes:Bearer:Token"]));
                });

builder.Services.AddAuthorization();


//services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
}).AddControllersAsServices().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    options.JsonSerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Apply pending migrations
app.ApplyDbMigration();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapFallbackToFile("index.html").AllowAnonymous(); // Sets index.html as the default landing page

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Asset}/{action=Index}/{id?}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("Version", config.GetSection("Version:Build"))
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs", "GoFish-log-{Date}.log"), rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

Log.Information("Starting web host");

app.Run();
