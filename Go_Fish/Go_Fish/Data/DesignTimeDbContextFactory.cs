using GoFishData;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace GoFish.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(local);Database=GoFish;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=false;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
