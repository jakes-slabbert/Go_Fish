using GoFish.Models;
using MediatR;

namespace Mediatr.Games.Commands
{
    public class CreateGameCommand : IRequest<Guid>
    {
        public string? Name { get; set; } = null;
        public List<PlayerDto>? Players { get; set; }
    }

}