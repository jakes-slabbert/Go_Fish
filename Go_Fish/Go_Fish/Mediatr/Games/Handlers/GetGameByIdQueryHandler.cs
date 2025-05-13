using MediatR;
using Mediatr.Games.Queries;
using Mediatr.Games.Responses;
using GoFish.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using GoFish.Models;

namespace Mediatr.Games.Handlers
{
    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, GamesGetByIdResponse>
    {
        private readonly AppDbContext _context;

        public GetGameByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GamesGetByIdResponse> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var game = await _context.Games
                    .Include(g => g.Players)
                    .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

                if (game == null) throw new ArgumentNullException(nameof(game));

                // Fetch deck cards (cards in the deck for this game)
                var deckCards = await _context.GameCards
                    .Where(gc => gc.GameId == request.Id && gc.InDeck && gc.DeletedOn == null)
                    .Join(_context.Cards,
                          gc => gc.CardId,
                          c => c.Id,
                          (gc, c) => new CardDto
                          {
                              Id = c.Id,
                              Rank = c.Rank
                          })
                    .ToListAsync(cancellationToken);

                return new GamesGetByIdResponse
                {
                    Game = new GameDto
                    {
                        Id = request.Id,
                        Name = game.Name,
                        IsCompleted = game.IsCompleted,
                        CompletedAt = game.CompletedAt,
                        WinnerPlayerId = game.WinnerPlayerId,
                        CurrentTurnPlayerId = game.CurrentTurnPlayerId,
                        Deck = deckCards,
                        Players = game.Players.Select(p => new PlayerDto
                        {
                            UserId = p.UserId,
                            Name = p.Name
                        }).ToList()
                    }
                };
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GetGameByIdQueryHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                throw;
            }
        }

    }
}