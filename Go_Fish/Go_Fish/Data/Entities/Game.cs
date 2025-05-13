namespace GoFish.Data.Entities
{
    public class Game : BaseEntity
    {
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }

        public Guid? WinnerPlayerId { get; set; }
        public virtual GamePlayer WinnerPlayer { get; set; }

        public Guid? CurrentTurnPlayerId { get; set; }
        public virtual GamePlayer CurrentTurnPlayer { get; set; }

        public virtual ICollection<GamePlayer> Players { get; set; }
        public virtual ICollection<GameCard> Cards { get; set; }
        public virtual ICollection<Move> Moves { get; set; }
    }
}
