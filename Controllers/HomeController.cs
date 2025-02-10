using Autos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Autos.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace Autos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AutoDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public HomeController(ILogger<HomeController> logger, AutoDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult Services()
		{
			return View();
		}

        [Authorize(Roles = "User,SuperAdmin,Admin")]
        public IActionResult Profile()
        {
            var CurUser = (from user in _context.Users
                           where user.UserName == _userManager.GetUserName(User)
                           select user).FirstOrDefault();

            return View(CurUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
