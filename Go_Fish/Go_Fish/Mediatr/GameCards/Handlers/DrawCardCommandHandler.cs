using GoFish.Data;
using GoFish.Data.Entities;
using GoFish.Mediatr.GameCards.Commands;
using GoFish.Models;
using GoFish.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoFish.Mediatr.GameCards.Handlers
{
    public class DrawCardCommandHandler : IRequestHandler<DrawCardCommand, List<CardDto>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DrawCardCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<CardDto>> Handle(DrawCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var game = await _context.Games
                    .Include(g => g.Players)
                    .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

                var gameCards = await _context.GameCards
                    .Where(g => g.GameId == request.GameId)
                    .ToListAsync(cancellationToken);

                var currentUserId = _currentUserService.UserId ?? throw new Exception("User not logged in");

                var currentPlayer = game.Players.FirstOrDefault(p => p.UserId == currentUserId);
                if (currentPlayer == null) throw new Exception("Player not found");

                var newCards = new List<GameCard>();

                bool initialDeal = gameCards.All(p => p.InDeck);
                int cardsPerPlayer = game.Players.Count <= 2 ? 7 : 5;

                if (initialDeal)
                {
                    var random = new Random();
                    foreach (var player in game.Players)
                    {
                        for (int i = 0; i < cardsPerPlayer; i++)
                        {
                            var card = DrawRandomCard(gameCards, random, player.Id);
                            _context.Attach(card);
                            newCards.Add(card);
                        }
                    }
                }
                else
                {
                    if (gameCards.Any())
                    {
                        var targetPlayerId = request.PlayerId ?? currentPlayer.Id;
                        var card = DrawRandomCard(gameCards, new Random(), targetPlayerId);
                        _context.Attach(card);
                        newCards.Add(card);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return newCards.Select(card => new CardDto
                {
                    Id = card.CardId,
                    Rank = card.Card.Rank,
                    //Suit = card., TODO: Figure this out
                    PlayerId = card.OwnedByGamePlayerId
                }).ToList();
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(DrawCardCommandHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                throw;
            }
        }

        private static GameCard DrawRandomCard(List<GameCard> gameCards, Random random, Guid gamePlayerId)
        {
            var availableCards = gameCards.Where(c => c.InDeck).ToList();
            if (!availableCards.Any()) throw new Exception("No cards left in deck.");

            var index = random.Next(availableCards.Count);
            var card = availableCards[index];
            card.InDeck = false;
            card.OwnedByGamePlayerId = gamePlayerId;
            return card;
        }
    }
}
