using MediatR;
using Mediatr.GameCards.Commands;
using Mediatr.GameCards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GameCards.Handlers
{
    public class GameCardsPatchHandler : IRequestHandler<GameCardsPatchCommand, GameCardsPatchResponse>
    {
        public Task<GameCardsPatchResponse> Handle(GameCardsPatchCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}