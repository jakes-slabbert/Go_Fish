using MediatR;
using Mediatr.Moves.Responses;

namespace Mediatr.Moves.Commands
{
    public class MovesCreateCommand : IRequest<MovesCreateResponse>
    {
    }
}