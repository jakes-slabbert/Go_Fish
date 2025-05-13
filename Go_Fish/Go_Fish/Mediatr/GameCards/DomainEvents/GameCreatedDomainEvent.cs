using MediatR;

namespace GoFish.Mediatr.GameCards.DomainEvents
{
    public class GameCreatedDomainEvent : INotification
    {
        public Guid GameId { get; }

        public GameCreatedDomainEvent(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
