using MediatR;
using Mediatr.GameCards.Queries;
using Mediatr.GameCards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GameCards.Handlers
{
    public class GameCardsGetByIdHandler : IRequestHandler<GameCardsGetByIdQuery, GameCardsGetByIdResponse>
    {
        public Task<GameCardsGetByIdResponse> Handle(GameCardsGetByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}