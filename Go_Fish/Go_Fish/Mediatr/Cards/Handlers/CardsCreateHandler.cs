using MediatR;
using Mediatr.Cards.Commands;
using Mediatr.Cards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Cards.Handlers
{
    public class CardsCreateHandler : IRequestHandler<CardsCreateCommand, CardsCreateResponse>
    {
        public Task<CardsCreateResponse> Handle(CardsCreateCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}