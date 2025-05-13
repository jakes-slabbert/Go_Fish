using GoFishControllers;
using GoFish.Data;
using Mediatr.Games.Commands;
using Mediatr.Games.Queries;
using Mediatr.Games.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoFish.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [HttpPost]
        public async Task<ActionResult<GamesGetByIdResponse>> CreateGame([FromBody] CreateGameCommand command, CancellationToken cancellationToken)
        {
            var gameId = await _mediator.Send(command, cancellationToken);
            var game = await _mediator.Send(new GetGameByIdQuery(gameId), cancellationToken);

            if (game == null)
                return NotFound();

            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
        }

        /// <summary>
        /// Get a game by its ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GamesGetByIdResponse>> GetGameById(Guid id, CancellationToken cancellationToken)
        {
            var game = await _mediator.Send(new GetGameByIdQuery(id), cancellationToken);

            if (game == null)
                return NotFound();

            return Ok(game);
        }
    }
}
