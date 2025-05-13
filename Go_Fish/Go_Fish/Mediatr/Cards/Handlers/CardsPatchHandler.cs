using MediatR;
using Mediatr.Cards.Commands;
using Mediatr.Cards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Cards.Handlers
{
    public class CardsPatchHandler : IRequestHandler<CardsPatchCommand, CardsPatchResponse>
    {
        public Task<CardsPatchResponse> Handle(CardsPatchCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}