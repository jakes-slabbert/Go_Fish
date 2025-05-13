using MediatR;
using Mediatr.Moves.Queries;
using Mediatr.Moves.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Moves.Handlers
{
    public class MovesGetByIdHandler : IRequestHandler<MovesGetByIdQuery, MovesGetByIdResponse>
    {
        public Task<MovesGetByIdResponse> Handle(MovesGetByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}