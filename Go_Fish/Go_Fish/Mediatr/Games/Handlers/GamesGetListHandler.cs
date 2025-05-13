using MediatR;
using Mediatr.Games.Queries;
using Mediatr.Games.Responses;
using GoFish.Services.CurrentUser;
using GoFish.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;
using GoFish.Models;

namespace Mediatr.Games.Handlers
{
    public class GamesGetListHandler : IRequestHandler<GamesGetListQuery, GamesGetListResponse>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;

        public GamesGetListHandler(AppDbContext appDbContext, ICurrentUserService currentUserService)
        {
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<GamesGetListResponse> Handle(GamesGetListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _currentUserService.UserId ?? throw new ArgumentNullException(nameof(_currentUserService.UserId));

                var games = _appDbContext.Games
                    .Include(g => g.Players)
                    .Where(a => a.Players.Select(p => p.UserId).ToList().Contains(userId) && a.DeletedOn == null && a.IsCompleted == false)
                    .Select(a => new GameDto 
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Players = a.Players.Select(p => new PlayerDto 
                        {
                            Id = p.Id,
                            UserId = p.UserId,
                            Name = p.Name
                        }).ToList(),

                    });

                return new GamesGetListResponse
                {
                    Games = [.. games]
                };
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(GamesGetListHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                return new GamesGetListResponse
                {
                    Games = []
                };
            }
        }
    }
}