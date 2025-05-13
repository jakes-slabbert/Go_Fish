using MediatR;
using Mediatr.Games.Commands;
using Mediatr.Games.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Games.Handlers
{
    public class GamesPatchHandler : IRequestHandler<GamesPatchCommand, GamesPatchResponse>
    {
        public Task<GamesPatchResponse> Handle(GamesPatchCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}