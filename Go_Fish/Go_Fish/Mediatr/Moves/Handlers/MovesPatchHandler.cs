using MediatR;
using Mediatr.Moves.Commands;
using Mediatr.Moves.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Moves.Handlers
{
    public class MovesPatchHandler : IRequestHandler<MovesPatchCommand, MovesPatchResponse>
    {
        public Task<MovesPatchResponse> Handle(MovesPatchCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}