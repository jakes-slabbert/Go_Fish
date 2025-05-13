using MediatR;
using Mediatr.Games.Commands;
using Mediatr.Games.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Games.Handlers
{
    public class GamesCreateHandler : IRequestHandler<GamesCreateCommand, GamesCreateResponse>
    {
        public Task<GamesCreateResponse> Handle(GamesCreateCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}