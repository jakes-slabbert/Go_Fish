

using GoFish.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace GoFish.Data
{
    public static class DatabaseExtensions
    {
        public static void ApplyDbMigration([NotNull] this IApplicationBuilder app)
        {
            var hostingEnv = app.ApplicationServices.GetService<IWebHostEnvironment>();
            var serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var config = new ConfigurationBuilder().SetBasePath(hostingEnv.ContentRootPath).AddJsonFile("appsettings.json", optional: false).Build();

            var userRoles = services.GetServices<AppRole>().ToList();

            var context = services.GetRequiredService<AppDbContext>();
            Log.Information($"Pending Migration: {string.Join(", ", context.Database.GetPendingMigrations())}");
            context.Database.Migrate();
            Log.Information("Applied all Migrations");
            Initialize(context, userRoles);
        }

        internal static void Initialize(AppDbContext db, List<AppRole> userRoles)
        {
            Log.Information("Intialising MRD - {For}", db.Database.GetDbConnection().Database);
            // Create the SysAdmin Users
            var user = EnsureUser(db, "Jaco", "jaco@fusiontree.co.za");
            CreateRoles(userRoles, db, user);

            user = EnsureUser(db, "FusionTree Support", "support@fusiontree.co.za");
            CreateRoles(userRoles, db, user);

            user = EnsureUser(db, "Silvan Thus", "silvan@tmm.nl");
            CreateRoles(userRoles, db, user);

            Log.Information("Done with MRD - {For}", db.Database.GetDbConnection().Database);
        }

        private static void CreateRoles(List<AppRole> userRoles, AppDbContext db, Guid user)
        {
            foreach (AppRole item in userRoles)
                //foreach (var role in item.GetAll())
                    EnsureRole(db, user, item.Name);
        }

        private static Guid EnsureUser(AppDbContext db, string name, string emailAddress, string password = "FusionTree@123")
        {
            var user = db.Users.Where(o => o.Email == emailAddress).FirstOrDefault();
            if (user == null)
            {
                PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
                // Hash the Password Secret:
                string passwordHash = passwordHasher.HashPassword(user, password);

                user = new AppUser
                {
                    EmailConfirmed = true,
                    Email = emailAddress,
                    LockoutEnabled = false,
                    UserName = emailAddress,
                    PasswordHash = passwordHash,
                    NormalizedEmail = emailAddress.ToUpper(),
                    NormalizedUserName = emailAddress.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Name = name
                };
                var changeTracker = db.Users.Add(user);
                var result = db.SaveChanges(true);
            }

            return user.Id;
        }

        private static void EnsureRole(AppDbContext db, Guid userId, string roleName)
        {
            var role = db.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                role = new AppRole { Name = roleName };
                db.Roles.Add(role);
                db.SaveChanges();
            }

            var userRole = db.UserRoles.FirstOrDefault(o => o.UserId == userId && o.RoleId == role.Id);
            if (userRole == null)
            {
                db.UserRoles.Add(new GoFish.Data.Entities.AppUserRole() { RoleId = role.Id, UserId = userId });
                db.SaveChanges();
            }
        }
    }
}
