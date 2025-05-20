using GoFish.Data;
using GoFish.Mediatr.GameCards.Commands;
using GoFish.Mediatr.GameCards.Responses;
using GoFish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoFish.Mediatr.GameCards.Handlers
{
    public class AskCardHandler : IRequestHandler<AskCardCommand, AskCardResult>
    {
        private readonly AppDbContext _context;

        public AskCardHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AskCardResult> Handle(AskCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var message = string.Empty;

                var game = _context.Games
                    .Include(g => g.Players)
                    .Single(g => g.Id == request.GameId);

                if (game == null)
                    return new AskCardResult { Success = false, Message = "Game not found." };

                var askingPlayer = game.Players.FirstOrDefault(p => p.Id == request.RequestingPlayerId);
                var targetPlayer = game.Players.FirstOrDefault(p => p.Id == request.TargetPlayerId);

                if (askingPlayer == null || targetPlayer == null)
                    return new AskCardResult { Success = false, Message = "Player not found." };

                if (game.CurrentTurnPlayerId != request.RequestingPlayerId)
                    return new AskCardResult { Success = false, Message = "It's not your turn." };

                var askingPlayerCards = _context.GameCards.Include(g => g.Card).Where(g => g.OwnedByGamePlayerId == askingPlayer.Id && g.GameId == game.Id).ToList();
                var targetPlayerCards = _context.GameCards.Include(g => g.Card).Where(g => g.OwnedByGamePlayerId == targetPlayer.Id && g.GameId == game.Id).ToList();

                var matchingCards = targetPlayerCards.Where(g => g.Card.Rank == request.Rank);

                if (matchingCards.Any())
                {
                    // Transfer cards
                    foreach (var card in matchingCards)
                    {
                        card.OwnedByGamePlayerId = askingPlayer.Id;
                        _context.Attach(card);
                    }

                    message = $"{askingPlayer.Name} got {matchingCards.Count()} card(s) of {request.Rank} from {targetPlayer.Name}.";

                    // Player gets another turn
                }
                else
                {
                    // Go fish
                    var deckCards = _context.GameCards.Include(g => g.Card).Where(g => g.OwnedByGamePlayerId == null && g.GameId == game.Id).ToList();

                    if (deckCards.Any())
                    {
                        var drawnCard = deckCards[new Random().Next(0, deckCards.Count - 1)];
                        drawnCard.OwnedByGamePlayerId = askingPlayer.Id;
                        _context.Attach(drawnCard);

                        message = $"{askingPlayer.Name} asked for {request.Rank} and went fishing.";

                        if (drawnCard.Card.Rank != request.Rank)
                        {
                            // Turn ends
                            game.CurrentTurnPlayerId = targetPlayer.Id;
                            _context.Attach(game);
                        }
                        else
                        {
                            message = $"{askingPlayer.Name} drew the rank they asked for! They go again.";
                        }
                    }
                    else
                    {
                        message = $"{askingPlayer.Name} asked for {request.Rank} but the deck is empty.";
                        game.CurrentTurnPlayerId = targetPlayer.Id;
                        _context.Attach(game);
                    }
                }

                await _context.SaveChangesAsync();

                // Reload fresh state to return
                var updatedCards = _context.GameCards
                    .Include(gc => gc.Card)
                    .Where(gc => gc.GameId == game.Id)
                    .Select(gc => new CardDto
                    {
                        Id = gc.CardId,
                        Rank = gc.Card.Rank,
                        Suit = $"{gc.Suite}",
                        PlayerId = gc.OwnedByGamePlayerId,

                    })
                    .ToList();

                // Determine if the game is complete
                var gameIsOver = updatedCards.All(gc => gc.PlayerId != null) &&
                                 game.Players.All(p => updatedCards.Any(gc => gc.PlayerId == p.Id));

                return new AskCardResult
                {
                    Success = true,
                    Message = message,
                    UpdatedCards = updatedCards,
                    GameIsOver = gameIsOver,
                    NextPlayerId = game.CurrentTurnPlayerId.HasValue ? game.CurrentTurnPlayerId.Value : throw new ArgumentNullException(nameof(game.CurrentTurnPlayerId))
                };
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(AskCardHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                throw;
            }
        }
    }
}
