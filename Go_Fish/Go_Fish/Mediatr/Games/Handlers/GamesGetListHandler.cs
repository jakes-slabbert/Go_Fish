using MediatR;
using Mediatr.Games.Queries;
using Mediatr.Games.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Games.Handlers
{
    public class GamesGetListHandler : IRequestHandler<GamesGetListQuery, GamesGetListResponse>
    {
        public Task<GamesGetListResponse> Handle(GamesGetListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}