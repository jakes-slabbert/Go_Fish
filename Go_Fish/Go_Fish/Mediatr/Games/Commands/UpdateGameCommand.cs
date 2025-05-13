using MediatR;

namespace Mediatr.Games.Commands
{
    public record UpdateGameCommand(Guid Id, string Name, Guid UpdatedById) : IRequest<bool>;
}