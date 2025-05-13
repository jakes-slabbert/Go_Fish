using MediatR;
using Mediatr.Moves.Responses;

namespace Mediatr.Moves.Queries
{
    public class MovesGetByIdQuery : IRequest<MovesGetByIdResponse>
    {
    }
}