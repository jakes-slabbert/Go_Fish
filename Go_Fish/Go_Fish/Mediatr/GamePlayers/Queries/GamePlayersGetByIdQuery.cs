using MediatR;
using Mediatr.GamePlayers.Responses;

namespace Mediatr.GamePlayers.Queries
{
    public class GamePlayersGetByIdQuery : IRequest<GamePlayersGetByIdResponse>
    {
    }
}