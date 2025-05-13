namespace GoFish.Data.Entities
{
    public class Card
    {
        public int Id { get; set; } // 2–14 (2-10, J=11, Q=12, K=13, A=14)
        public string Rank { get; set; }

        public ICollection<GameCard> GameCards { get; set; }
    }
}
