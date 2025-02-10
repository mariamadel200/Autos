using Autos.Data;
using Autos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autos.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly AutoDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserController(AutoDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Dashboard()
        {

            return View();
        }
        public IActionResult Sales()
        {
            var CurUser = (from user in _context.Users
                           where user.UserName == _userManager.GetUserName(User)
                           select user).FirstOrDefault();

            var sales= (from sale in _context.Sales
                        where sale.UserId == CurUser.Id
                        select sale)
                        .Include(s =>s.Payment)
                        .ToList();

          
            return View(sales);
        }

        public IActionResult Details(int id)
        {
            var Cursale = (from sale in _context.Sales
                         where sale.Id == id
                         select sale)
                         .Include(s => s.VehiclesForSales)
                         .FirstOrDefault();
            var Vehicles = Cursale.VehiclesForSales;

            return View(Vehicles);
        }

        public IActionResult Rentals()
        {
            var CurUser = (from user in _context.Users
                           where user.UserName == _userManager.GetUserName(User)
                           select user).FirstOrDefault();

            var rentales = (from rental in _context.Rentals
                         where rental.UserId == CurUser.Id
                         select rental)
                        .Include(s => s.Payment)
                        .ToList();


            return View(rentales);
        }

        public IActionResult RentDetails(int id)
        {
            var Cursale = (from rent in _context.Rentals
                           where rent.Id == id
                           select rent)
                         .Include(s => s.VehiclesForRents)
                         .FirstOrDefault();
            var Vehicles = Cursale.VehiclesForRents;

            return View(Vehicles);
        }

    }
}
