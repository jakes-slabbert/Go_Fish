namespace GoFish.Models
{
    public class GameDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public Guid? WinnerPlayerId { get; set; }

        public Guid? CurrentTurnPlayerId { get; set; }

        public List<CardDto> Deck { get; set; }

        public List<PlayerDto> Players { get; set; }
    }
}
