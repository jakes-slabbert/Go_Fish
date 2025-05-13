namespace GoFish.Models
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Rank { get; set; } // e.g., "A", "2", ..., "K"
        public string Suit { get; set; } // e.g., "Hearts", "Spades", "Clubs", "Diamonds"
        public Guid? PlayerId { get; set; }
    }
}
