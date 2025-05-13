using MediatR;
using Mediatr.Moves.Commands;
using Mediatr.Moves.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Moves.Handlers
{
    public class MovesCreateHandler : IRequestHandler<MovesCreateCommand, MovesCreateResponse>
    {
        public Task<MovesCreateResponse> Handle(MovesCreateCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}