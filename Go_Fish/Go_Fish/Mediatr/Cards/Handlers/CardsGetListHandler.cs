using MediatR;
using Mediatr.Cards.Queries;
using Mediatr.Cards.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediatr.Cards.Handlers
{
    public class CardsGetListHandler : IRequestHandler<CardsGetListQuery, CardsGetListResponse>
    {
        public Task<CardsGetListResponse> Handle(CardsGetListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}