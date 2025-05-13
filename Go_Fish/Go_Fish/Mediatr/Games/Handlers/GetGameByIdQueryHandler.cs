using MediatR;
using Mediatr.Games.Queries;
using Mediatr.Games.Responses;
using GoFish.Data;
using Microsoft.EntityFrameworkCore;

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
            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (game == null) return null;

            return new GamesGetByIdResponse
            {
                Id = game.Id,
                Name = game.Name,
                CreatedOn = game.CreatedOn,
                CreatedById = game.CreatedById,
                IsCompleted = game.IsCompleted,
                CompletedAt = game.CompletedAt,
                WinnerPlayerId = game.WinnerPlayerId,
                CurrentTurnPlayerId = game.CurrentTurnPlayerId
            };
        }
    }
}