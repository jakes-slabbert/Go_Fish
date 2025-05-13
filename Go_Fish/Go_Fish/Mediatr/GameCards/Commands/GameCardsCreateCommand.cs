using MediatR;
using Mediatr.GameCards.Responses;

namespace Mediatr.GameCards.Commands
{
    public class GameCardsCreateCommand : IRequest<GameCardsCreateResponse>
    {
    }
}