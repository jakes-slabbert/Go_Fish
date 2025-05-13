using MediatR;
using Mediatr.GamePlayers.Commands;
using Mediatr.GamePlayers.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GamePlayers.Handlers
{
    public class GamePlayersPatchHandler : IRequestHandler<GamePlayersPatchCommand, GamePlayersPatchResponse>
    {
        public Task<GamePlayersPatchResponse> Handle(GamePlayersPatchCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}