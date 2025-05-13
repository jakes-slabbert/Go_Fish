namespace GoFish.Data.Entities
{
    public class Game : BaseEntity
    {
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }

        public Guid? WinnerPlayerId { get; set; }
        public GamePlayer WinnerPlayer { get; set; }

        public Guid? CurrentTurnPlayerId { get; set; }
        public GamePlayer CurrentTurnPlayer { get; set; }

        public ICollection<GamePlayer> Players { get; set; }
        public ICollection<GameCard> Cards { get; set; }
        public ICollection<Move> Moves { get; set; }
    }
}
