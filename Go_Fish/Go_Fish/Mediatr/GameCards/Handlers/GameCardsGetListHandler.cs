using MediatR;
using Mediatr.GameCards.Queries;
using Mediatr.GameCards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GameCards.Handlers
{
    public class GameCardsGetListHandler : IRequestHandler<GameCardsGetListQuery, GameCardsGetListResponse>
    {
        public Task<GameCardsGetListResponse> Handle(GameCardsGetListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}