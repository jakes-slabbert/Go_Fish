using GoFishData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoFishControllers
{
    [Authorize]
    public class BaseController : Controller
    {

        public BaseController(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public AppDbContext DbContext { get; set; }

        //public IHubContext<RealtimeMessageHub, IRealtimeMessageClient> RealtimeMessageHub { get; set; }

    }
}
