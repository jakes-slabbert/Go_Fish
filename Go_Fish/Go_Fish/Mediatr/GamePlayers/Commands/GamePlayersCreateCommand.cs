using MediatR;
using Mediatr.GamePlayers.Responses;

namespace Mediatr.GamePlayers.Commands
{
    public class GamePlayersCreateCommand : IRequest<GamePlayersCreateResponse>
    {
    }
}