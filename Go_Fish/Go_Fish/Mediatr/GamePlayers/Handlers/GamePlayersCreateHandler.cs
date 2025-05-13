using MediatR;
using Mediatr.GamePlayers.Commands;
using Mediatr.GamePlayers.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GamePlayers.Handlers
{
    public class GamePlayersCreateHandler : IRequestHandler<GamePlayersCreateCommand, GamePlayersCreateResponse>
    {
        public Task<GamePlayersCreateResponse> Handle(GamePlayersCreateCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}