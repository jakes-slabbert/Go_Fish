using GoFishData.Entities;

namespace GoFish.Data.Entities
{
    public class GamePlayer : BaseEntity
    {
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }

        public Guid? UserId { get; set; }
        public virtual AppUser User { get; set; }

        public bool IsComputer { get; set; }
        public int PlayerOrder { get; set; }

        public virtual ICollection<GameCard> Cards { get; set; }
        public virtual ICollection<Move> MovesMade { get; set; }
    }
}
