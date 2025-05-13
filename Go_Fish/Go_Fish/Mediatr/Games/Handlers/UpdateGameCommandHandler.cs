using MediatR;
using Mediatr.Games.Commands;
using GoFish.Data;
using Serilog;

namespace Mediatr.Games.Handlers
{
    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateGameCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var game = await _context.Games.FindAsync(new object[] { request.Id }, cancellationToken);
                if (game == null) return false;

                game.Name = request.Name;
                game.UpdatedOn = DateTimeOffset.UtcNow;
                game.UpdatedBy = await _context.Users.FindAsync(new object[] { request.UpdatedById }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception exception)
            {
                Log.Logger.Error($"{nameof(UpdateGameCommandHandler)} failed. \r\n Message: \r\n {exception.Message} \r\n Stacktrace: \r\n {exception.StackTrace} \r\n --------------------------- End of Error ---------------------------");
                return false;
            }
        }
    }
}