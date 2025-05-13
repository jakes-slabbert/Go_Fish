using MediatR;
using Mediatr.Cards.Responses;

namespace Mediatr.Cards.Commands
{
    public class CardsCreateCommand : IRequest<CardsCreateResponse>
    {
    }
}