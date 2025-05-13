using GoFish.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoFish.Services
{
    public static class CardSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var ranks = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            var cards = ranks.Select((rank, index) => new Card
            {
                Id = index + 2,
                Rank = rank
            }).ToList();

            modelBuilder.Entity<Card>().HasData(cards);
        }
    }
}
