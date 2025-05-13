using MediatR;
using Mediatr.Moves.Queries;
using Mediatr.Moves.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Moves.Handlers
{
    public class MovesGetListHandler : IRequestHandler<MovesGetListQuery, MovesGetListResponse>
    {
        public Task<MovesGetListResponse> Handle(MovesGetListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}