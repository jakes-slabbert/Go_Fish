using MediatR;
using Mediatr.Games.Responses;

namespace Mediatr.Games.Queries
{
    public record GetGameByIdQuery(Guid Id) : IRequest<GamesGetByIdResponse>;
}