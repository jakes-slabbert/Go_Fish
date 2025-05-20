using GoFish.Models;
using MediatR;

namespace GoFish.Mediatr.GameCards.Commands
{
    public class DrawCardCommand : IRequest<List<CardDto>>
    {
        public Guid GameId { get; set; }

        public Guid? PlayerId { get; set; }
    }
}
