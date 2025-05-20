using GoFish.Models;

namespace GoFish.Mediatr.GameCards.Responses
{
    public class AskCardResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<CardDto> UpdatedCards { get; set; } = new();
        public bool GameIsOver { get; set; }
        public Guid NextPlayerId { get; set; }
    }
}
