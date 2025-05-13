using MediatR;
using Mediatr.GameCards.Commands;
using Mediatr.GameCards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GameCards.Handlers
{
    public class GameCardsCreateHandler : IRequestHandler<GameCardsCreateCommand, GameCardsCreateResponse>
    {
        public Task<GameCardsCreateResponse> Handle(GameCardsCreateCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}