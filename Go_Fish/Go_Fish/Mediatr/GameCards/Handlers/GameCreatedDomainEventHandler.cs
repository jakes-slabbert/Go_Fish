using GoFish.Data;
using GoFish.Data.Entities;
using GoFish.Data.Enumerations;
using GoFish.Mediatr.GameCards.DomainEvents;
using GoFish.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoFish.Mediatr.GameCards.Handlers
{
    public class GameCreatedDomainEventHandler : INotificationHandler<GameCreatedDomainEvent>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GameCreatedDomainEventHandler(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task Handle(GameCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var allCards = await _context.Cards.ToListAsync(cancellationToken);
                var suits = Enum.GetValues(typeof(Suite)).Cast<Suite>(); ; // Suites needed

                var gameCards = new List<GameCard>();

                foreach (var card in allCards)
                {
                    foreach (var suit in suits)
                    {
                        gameCards.Add(new GameCard
                        {
                            GameId = notification.GameId,
                            CardId = card.Id,
                            Name = $"{card.Rank} of {suit}",
                            InDeck = true,
                            CreatedOn = DateTimeOffset.UtcNow,
                            CreatedById = currentUserId.HasValue ? currentUserId.Value : throw new ArgumentNullException(nameof(currentUserId)),
                            Suite = suit
                        });
                    }
                }

                await _context.GameCards.AddRangeAsync(gameCards, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GameCreatedDomainEventHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                throw;
            }
        }
    }
}
