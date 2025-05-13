using MediatR;
using Mediatr.Games.Queries;
using Mediatr.Games.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Games.Handlers
{
    public class GamesGetByIdHandler : IRequestHandler<GamesGetByIdQuery, GamesGetByIdResponse>
    {
        public Task<GamesGetByIdResponse> Handle(GamesGetByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}