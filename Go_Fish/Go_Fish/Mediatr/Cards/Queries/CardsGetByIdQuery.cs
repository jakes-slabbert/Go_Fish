using MediatR;
using Mediatr.Cards.Responses;

namespace Mediatr.Cards.Queries
{
    public class CardsGetByIdQuery : IRequest<CardsGetByIdResponse>
    {
    }
}