using GoFish.Data.Entities;
using GoFish.Services;
using GoFish.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoFish.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<GameCard> GameCards { get; set; }
        public DbSet<Move> Moves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique Card Rank
            builder.Entity<Card>()
                .HasIndex(c => c.Rank)
                .IsUnique();

            builder.Entity<GamePlayer>()
                .HasOne(p => p.User)
                .WithMany(u => u.GamePlayers)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GamePlayer>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.WinnerPlayer)
                .WithMany()
                .HasForeignKey(g => g.WinnerPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.CurrentTurnPlayer)
                .WithMany()
                .HasForeignKey(g => g.CurrentTurnPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GameCard>()
                .HasOne(g => g.OwnedByGamePlayer)
                .WithMany()
                .HasForeignKey(g => g.OwnedByGamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GameCard>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Move>()
                .HasOne(m => m.AskingPlayer)
                .WithMany(p => p.MovesMade)
                .HasForeignKey(m => m.AskingPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Move>()
                .HasOne(m => m.TargetPlayer)
                .WithMany()
                .HasForeignKey(m => m.TargetPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Move>()
                .HasOne(m => m.CreatedBy)
                .WithMany()
                .HasForeignKey(m => m.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Move>()
                .HasOne(m => m.TargetPlayer)
                .WithMany()
                .HasForeignKey(m => m.TargetPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
            CardSeeder.Seed(builder);
        }
    }
}
