using MediatR;
using Mediatr.GameCards.Responses;

namespace Mediatr.GameCards.Commands
{
    public class GameCardsPatchCommand : IRequest<GameCardsPatchResponse>
    {
    }
}