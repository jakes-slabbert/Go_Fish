using MediatR;
using Mediatr.Cards.Queries;
using Mediatr.Cards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Cards.Handlers
{
    public class CardsGetByIdHandler : IRequestHandler<CardsGetByIdQuery, CardsGetByIdResponse>
    {
        public Task<CardsGetByIdResponse> Handle(CardsGetByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}