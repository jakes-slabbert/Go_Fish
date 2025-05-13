using GoFish.Mediatr.GameCards.Responses;
using MediatR;

namespace GoFish.Mediatr.GameCards.Commands
{
    public class AskCardCommand : IRequest<AskCardResult>
    {
        public Guid GameId { get; set; }
        public Guid RequestingPlayerId { get; set; }
        public Guid TargetPlayerId { get; set; }
        public string Rank { get; set; }
    }
}
