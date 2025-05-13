using GoFishControllers;
using GoFish.Data;
using Mediatr.Games.Commands;
using Mediatr.Games.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using GoFish.Mediatr.GameCards.Commands;

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

        [HttpPost]
        public async Task<IActionResult> DrawCard(Guid gameId, Guid? playerId = null)
        {
            try
            {
                var result = await _mediator.Send(new DrawCardCommand
                {
                    GameId = gameId,
                    PlayerId = playerId
                });

                return Json(result); // Return cards to update UI
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GamesController)} - {nameof(DrawCard)} failed. \r\n Message: \r\n {exception.Message} \r\n StackTrace: \r\n {exception.StackTrace}");
                return StatusCode(500, "Something went wrong.");
            }
        }
    }
}
