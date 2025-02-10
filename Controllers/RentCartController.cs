using Autos.Data;
using Autos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Autos.Controllers
{
    [Authorize(Roles = "User")]
    public class RentCartController : Controller
    {
        private readonly AutoDbContext dbContext;
        private readonly UserManager<User> _userManager;
        public RentCartController(AutoDbContext Context, UserManager<User> userManager)
        {
            dbContext = Context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            //var username = HttpContext.Session.GetString("UserName");
            var CurUser = (from user in dbContext.Users
                           where user.UserName == _userManager.GetUserName(User)
                           select user).FirstOrDefault();

            var CartItem = (from veh in dbContext.VehiclesForRent
                            where veh.Id == id
                            select veh).First();

            var cart = HttpContext.Session.Get<List<VehicleForRent>>("RentCart") ?? new List<VehicleForRent>();
            cart.Add(CartItem);
            HttpContext.Session.Set("RentCart", cart);

            return RedirectToAction("Search", "Rental");
        }
        public IActionResult ShowCart()
        {
            var cart = HttpContext.Session.Get<List<VehicleForRent>>("RentCart") ?? new List<VehicleForRent>();
            var cartid = cart.Select(c => c.Id).ToHashSet();
            var query = dbContext.VehiclesForRent.Where(v => cartid.Contains(v.Id));
            query = query.Include(q => q.Rentals);
            DateTime Start, End;
            DateTime.TryParse(HttpContext.Session.GetString("StartDate"), out Start);
            DateTime.TryParse(HttpContext.Session.GetString("EndDate"), out End);
            query = query.Where(q => !q.Rentals.Any(re => re.StartDate <= End && re.EndDate >= Start));

            return View(cart);
        }

        public IActionResult RemoveFromCart(int id)
        {

            var cart = HttpContext.Session.Get<List<VehicleForRent>>("RentCart") ?? new List<VehicleForRent>();

            var VehicleToRemove = cart.FirstOrDefault(veh => veh.Id == id);
            if (VehicleToRemove != null)
                cart.Remove(VehicleToRemove);

            // Save the updated cart back to session
            HttpContext.Session.Set("RentCart", cart);

            return RedirectToAction("ShowCart", "RentCart"); // Redirect to the product list or any other page
        }


        public IActionResult ProccedToPay()
        {
            var cart = HttpContext.Session.Get<List<VehicleForRent>>("RentCart") ?? new List<VehicleForRent>();
            if (cart.IsNullOrEmpty())
            {
                return RedirectToAction("Period", "Rental");
            }

            double PriceOfVehicles = 0;
            DateTime Start, End;
            DateTime.TryParse(HttpContext.Session.GetString("StartDate"), out Start);
            DateTime.TryParse(HttpContext.Session.GetString("EndDate"), out End);
            TimeSpan span= End.Subtract(Start);
            
            foreach (var veh in cart)
                PriceOfVehicles += span.Days*veh.PricePerDay;//need to do some calculations remember please عشان منروحش في داهيه
            ViewBag.TotalPrice = PriceOfVehicles;
            return View();
        }

        public IActionResult Done()
        {

            return View();
        }

        public IActionResult CheckOut(IFormCollection req)
        {
            ViewBag.Method = req["PaymentMethod"];
            ViewBag.Amount = req["Amount"];
            double PriceOfVehicles = 0;
            var cart = HttpContext.Session.Get<List<VehicleForRent>>("RentCart") ?? new List<VehicleForRent>();
            DateTime Start, End;
            DateTime.TryParse(HttpContext.Session.GetString("StartDate"), out Start);
            DateTime.TryParse(HttpContext.Session.GetString("EndDate"), out End);
            TimeSpan span = End.Subtract(Start);

            foreach (var veh in cart)
                PriceOfVehicles += span.Days * veh.PricePerDay;

           

            if (string.IsNullOrEmpty(ViewBag.Amount) || !double.TryParse(ViewBag.Amount, out double amount))
            {
                // Handle the case where Amount is not provided or is not a valid double
                ModelState.AddModelError("Amount", "The amount entered is not valid.");
                return RedirectToAction("ProccedToPay"); // Return the view with the validation error
            }
            if (Convert.ToDouble(ViewBag.Amount)> PriceOfVehicles)
            {
                // Handle the case where Amount entered is bigger than the total price
                ModelState.AddModelError("Amount", "The amount entered is bigger than the total price of vehicles");
                return RedirectToAction("ProccedToPay"); // Return the view with the validation error
            }

            Payment payment = new Payment();
            payment.TotalPrice = PriceOfVehicles;
            payment.Method = ViewBag.Method;
            payment.AmountLeft = PriceOfVehicles - Convert.ToDouble(ViewBag.Amount);

            dbContext.Add(payment);
            dbContext.SaveChanges();


            //var username = HttpContext.Session.GetString("UserName");
            //var user = dbContext.Users.FirstOrDefault(u => u.UserName == username);

            var CurUser = (from user in dbContext.Users
                           where user.UserName == _userManager.GetUserName(User)
                           select user).FirstOrDefault();

            
            //add list of vehichles
            foreach (var veh in cart)
            {
                dbContext.Entry(veh).State = EntityState.Unchanged;
            }
                Rental rental = new Rental();
            rental.PaymentId = payment.Id;
            rental.UserId = CurUser.Id;
            rental.Delivered = false;
            rental.StartDate = Start;
            rental.EndDate = End;
            rental.VehiclesForRents = cart;
            dbContext.Rentals.Add(rental);
            dbContext.SaveChanges();

            cart.Clear();
            HttpContext.Session.Set("RentCart", cart);
            HttpContext.Session.SetString("StartDate", "");
            HttpContext.Session.SetString("EndDate", "");

            return RedirectToAction("Done", "RentCart");
        }
    }
}
