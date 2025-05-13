using MediatR;
using Mediatr.GamePlayers.Queries;
using Mediatr.GamePlayers.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GamePlayers.Handlers
{
    public class GamePlayersGetByIdHandler : IRequestHandler<GamePlayersGetByIdQuery, GamePlayersGetByIdResponse>
    {
        public Task<GamePlayersGetByIdResponse> Handle(GamePlayersGetByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}