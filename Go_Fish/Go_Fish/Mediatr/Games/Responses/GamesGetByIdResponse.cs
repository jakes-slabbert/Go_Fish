namespace Mediatr.Games.Responses
{
    public class GamesGetByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public Guid? CurrentTurnPlayerId { get; set; }
    }
}