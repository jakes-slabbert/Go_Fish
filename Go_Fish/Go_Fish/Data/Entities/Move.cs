namespace GoFish.Data.Entities
{
    public class Move : BaseEntity
    {
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }

        public int TurnNumber { get; set; }

        public Guid AskingPlayerId { get; set; }
        public virtual GamePlayer AskingPlayer { get; set; }

        public Guid? TargetPlayerId { get; set; }
        public virtual GamePlayer TargetPlayer { get; set; }

        public int RequestedCardId { get; set; }
        public virtual Card RequestedCard { get; set; }

        public bool Success { get; set; }
        public bool DrewFromDeck { get; set; }
        public int CardsReceived { get; set; }
        public bool CompletedBook { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
