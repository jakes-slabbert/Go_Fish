using MediatR;
using Mediatr.GamePlayers.Queries;
using Mediatr.GamePlayers.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.GamePlayers.Handlers
{
    public class GamePlayersGetListHandler : IRequestHandler<GamePlayersGetListQuery, GamePlayersGetListResponse>
    {
        public Task<GamePlayersGetListResponse> Handle(GamePlayersGetListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}