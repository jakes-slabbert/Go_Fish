using MediatR;
using Mediatr.GameCards.Responses;

namespace Mediatr.GameCards.Queries
{
    public class GameCardsGetListQuery : IRequest<GameCardsGetListResponse>
    {
    }
}