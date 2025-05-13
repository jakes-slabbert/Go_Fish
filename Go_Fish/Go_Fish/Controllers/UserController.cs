using GoFish.Data;
using GoFish.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoFishControllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(AppDbContext dbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var data = DbContext.Users;

            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            return View("Edit");
        }

    }
}
