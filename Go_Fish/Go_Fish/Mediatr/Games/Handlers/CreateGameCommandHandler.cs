using MediatR;
using Mediatr.Games.Commands;
using GoFish.Data.Entities;
using GoFish.Data;
using GoFish.Services.CurrentUser;
using Serilog;

namespace Mediatr.Games.Handlers
{
    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Guid>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateGameCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    CreatedOn = DateTimeOffset.UtcNow,
                    CreatedById = currentUserId ?? throw new ArgumentNullException(nameof(currentUserId)),
                    Players = new List<GamePlayer>()
                };

                // Add current user as the first player
                game.Players.Add(new GamePlayer
                {
                    Id = Guid.NewGuid(),
                    Name = "You",
                    Game = game,
                    GameId = game.Id,
                    PlayerOrder = 0,
                    UserId = currentUserId,
                    CreatedOn = DateTimeOffset.UtcNow,
                    CreatedById = currentUserId ?? throw new ArgumentNullException(nameof(currentUserId)),
                });

                // Add provided players (excluding the current user)
                if (request.Players != null && request.Players.Any())
                {
                    int order = 1;
                    foreach (var player in request.Players)
                    {
                        game.Players.Add(new GamePlayer
                        {
                            Id = Guid.NewGuid(),
                            Name = player.Name,
                            Game = game,
                            GameId = game.Id,
                            PlayerOrder = order++,
                            IsComputer = player.UserId == null,
                            UserId = player.UserId,
                            CreatedOn = DateTimeOffset.UtcNow,
                            CreatedById = currentUserId ?? throw new ArgumentNullException(nameof(currentUserId)),
                        });
                    }
                }
                else
                {
                    // Add a computer player if no players provided
                    game.Players.Add(new GamePlayer
                    {
                        Id = Guid.NewGuid(),
                        Name = "Computer",
                        Game = game,
                        GameId = game.Id,
                        PlayerOrder = 1,
                        IsComputer = true,
                        CreatedOn = DateTimeOffset.UtcNow,
                        CreatedById = currentUserId ?? throw new ArgumentNullException(nameof(currentUserId)),
                    });
                }

                _context.Games.Add(game);
                await _context.SaveChangesAsync(cancellationToken);

                return game.Id;
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(CreateGameCommandHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                return Guid.Empty;
            }
        }
    }

}