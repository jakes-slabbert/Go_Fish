using GoFishControllers;
using GoFish.Data;
using Mediatr.Games.Commands;
using Mediatr.Games.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using GoFish.Mediatr.GameCards.Commands;
using Microsoft.EntityFrameworkCore;
using GoFish.Models;
using GoFish.Data.Enumerations;

namespace GoFish.Controllers
{
    [Route("[controller]")]
    public class GamesController : BaseController
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator, AppDbContext dbContext) : base(dbContext)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cancellationToken = new CancellationToken();
            var games = await _mediator.Send(new GamesGetListQuery(), cancellationToken);
            return View(games);
        }

        /// <summary>
        /// Create a new game.
        /// </summary>
        [HttpPost("CreateGame")]
        [IgnoreAntiforgeryToken]
        public async Task<string> CreateGame([FromBody] CreateGameCommand command)
        {
            var cancellationToken = new CancellationToken();
            var gameId = await _mediator.Send(command, cancellationToken);

            if (gameId == default)
                return JsonConvert.SerializeObject(new { Success = false });

            return JsonConvert.SerializeObject(new { Success = true, Game = gameId });
        }

        [HttpGet("View/{id:guid}")]
        public async Task<IActionResult> View(Guid id)
        {
            try
            {
                var cancellationToken = new CancellationToken();
                var game = await _mediator.Send(new GetGameByIdQuery(id), cancellationToken);

                if (game == null)
                    return NotFound();

                return View("View", game.Game); // Renders Views/Games/View.cshtml with the game data
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GamesController)} - {nameof(View)} failed. \r\n Message: \r\n {exception.Message} \r\n StackTrace: \r\n {exception.StackTrace}");
                return RedirectToAction(nameof(Index), "Games");
            }
        }

        [HttpPost("DrawCard")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DrawCard([FromBody] DrawCardCommand drawCardCommand)
        {
            try
            {
                var result = await _mediator.Send(drawCardCommand);

                return Json(result); // Return cards to update UI
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GamesController)} - {nameof(DrawCard)} failed. \r\n Message: \r\n {exception.Message} \r\n StackTrace: \r\n {exception.StackTrace}");
                return StatusCode(500, "Something went wrong.");
            }
        }

        [HttpPost("AskCard")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AskCard([FromBody] AskCardCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GamesController)} - {nameof(AskCard)} failed. \r\n Message: \r\n {exception.Message} \r\n StackTrace: \r\n {exception.StackTrace}");
                return StatusCode(500, "Something went wrong.");
            }
        }

        [HttpPost("ComputerTurn")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ComputerTurn([FromBody] Guid gameId)
        {
            var game = await DbContext.Games
                .Include(g => g.Players)
                    .ThenInclude(p => p.Cards)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null) return NotFound();

            var computer = game.Players.SingleOrDefault(p => p.Id == game.CurrentTurnPlayerId);
            if (computer == null) // TODO: Add Id checcking with current user
                return BadRequest("Invalid computer turn.");

            var opponent = game.Players.First(p => p.Id != computer.Id);
            var computerHand = DbContext.GameCards.Include(g => g.Card).Where(g => g.OwnedByGamePlayerId == game.CurrentTurnPlayerId);

            // Simple AI: randomly choose a rank from hand
            if (!computerHand.Any())
            {
                // No cards? Draw one
                var drawnCard = DbContext.GameCards.Where(g => g.OwnedByGamePlayerId == null && g.GameId == game.Id).FirstOrDefault();
                if (drawnCard != null)
                {
                    drawnCard.OwnedByGamePlayerId = computer.Id;
                    DbContext.Attach(drawnCard);
                }

                game.CurrentTurnPlayerId = opponent.Id;
                DbContext.Attach(game);
                await DbContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = $"{computer.Name} had no cards and drew from the deck.",
                    UpdatedCards = DbContext.GameCards.Include(g => g.Card).Where(g => g.GameId == game.Id).Select(g => new CardDto
                    {
                        Id = g.CardId,
                        Rank = g.Card.Rank,
                        Suit = $"{g.Suite}",
                        PlayerId = g.OwnedByGamePlayerId
                    }),
                    NextPlayerId = game.CurrentTurnPlayerId,
                    GameIsOver = false // TODO: complete this
                });
            }

            var random = new Random();
            var randomRank = computerHand.ToList()[random.Next(0, computerHand.Count() - 1)].Card.Rank;

            // Check opponent's hand
            var matchingCards = DbContext.GameCards.Include(c => c.Card).Where(c => c.GameId == game.Id && c.OwnedByGamePlayerId == opponent.Id && c.Card.Rank == randomRank).ToList();

            string message;

            if (matchingCards.Any())
            {
                foreach (var card in matchingCards)
                {
                    card.OwnedByGamePlayerId = computer.Id;
                    DbContext.Attach(card);
                }

                message = $"{computer.Name} asked for {randomRank}s and got {matchingCards.Count} card(s)!";
            }
            else
            {
                // Go Fish
                var drawnCard = DbContext.GameCards.Where(g => g.GameId == game.Id && g.OwnedByGamePlayerId == null).FirstOrDefault();
                if (drawnCard != null)
                {
                    drawnCard.OwnedByGamePlayerId = computer.Id;
                    DbContext.Attach(drawnCard);
                }

                game.CurrentTurnPlayerId = opponent.Id;
                DbContext.Attach(game);
                message = $"{computer.Name} asked for {randomRank}s but had to Go Fish.";
            }

            // Check for books, etc., and game over logic here...

            await DbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = message,
                UpdatedCards = DbContext.GameCards.Include(g => g.Card).Where(g => g.GameId == game.Id).Select(g => new CardDto 
                {
                    Id = g.CardId,
                    Rank = g.Card.Rank,
                    Suit = $"{g.Suite}",
                    PlayerId = g.OwnedByGamePlayerId
                }),
                NextPlayerId = game.CurrentTurnPlayerId,
                GameIsOver = false // TODO: complete this
            });
        }
    }
}
