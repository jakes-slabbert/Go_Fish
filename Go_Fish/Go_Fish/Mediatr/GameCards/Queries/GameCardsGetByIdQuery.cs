using MediatR;
using Mediatr.GameCards.Responses;

namespace Mediatr.GameCards.Queries
{
    public class GameCardsGetByIdQuery : IRequest<GameCardsGetByIdResponse>
    {
    }
}