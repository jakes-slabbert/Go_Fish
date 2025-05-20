namespace GoFish.Models
{
    public class AskCardRequest
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; } // The target player being asked
        public string Rank { get; set; }
    }
}
