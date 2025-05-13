namespace GoFish.Data.Entities
{
    public class GameCard : BaseEntity
    {
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }

        public int CardId { get; set; }
        public virtual Card Card { get; set; }

        public Guid? OwnedByGamePlayerId { get; set; }
        public virtual GamePlayer OwnedByGamePlayer { get; set; }

        public bool InDeck { get; set; } = true;
        public bool IsBooked { get; set; } = false;
        public bool IsVisibleToAll { get; set; } = false;
    }
}
