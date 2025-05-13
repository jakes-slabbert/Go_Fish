using MediatR;
using Mediatr.GamePlayers.Responses;

namespace Mediatr.GamePlayers.Commands
{
    public class GamePlayersPatchCommand : IRequest<GamePlayersPatchResponse>
    {
    }
}