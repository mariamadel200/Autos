using Autos.Data;
using Autos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Autos.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly AutoDbContext dbContext;
        private readonly UserManager<User> _userManager;
        public CartController(AutoDbContext Context, UserManager<User> userManager)
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

            var CartItem = (from veh in dbContext.VehiclesForSale
                            where veh.Id == id
                            select veh).First();

            var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();
            cart.Add(CartItem);
            HttpContext.Session.Set("ShoppingCart", cart);

            return RedirectToAction("Search", "Search");
        }
        public IActionResult ShowCart()
        {
            var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();

            var vid=dbContext.VehiclesForSale.Where(v =>v.Available==false).Select(v => v.Id).ToHashSet();
            cart=cart.Where(v => !vid.Contains(v.Id)).ToList();
            return View(cart);
        }

        public IActionResult RemoveFromCart(int id)
        {

            var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();

            var VehicleToRemove = cart.FirstOrDefault(veh => veh.Id == id);
            if (VehicleToRemove != null)
                cart.Remove(VehicleToRemove);

            // Save the updated cart back to session
            HttpContext.Session.Set("ShoppingCart", cart);

            return RedirectToAction("ShowCart", "Cart"); // Redirect to the product list or any other page
        }

        
        public IActionResult ProccedToPay()
        {
            var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();
            if (cart.IsNullOrEmpty())
            {
                return RedirectToAction("Search", "Search");
            }
            double PriceOfVehicles = 0;
           
            foreach (var veh in cart)
                PriceOfVehicles += veh.Price;
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
            var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();
            foreach (var veh in cart)
                PriceOfVehicles += veh.Price;

            if (string.IsNullOrEmpty(ViewBag.Amount) || !double.TryParse(ViewBag.Amount, out double amount))
            {
                // Handle the case where Amount is not provided or is not a valid double
                ModelState.AddModelError("Amount", "The amount entered is not valid.");
                return RedirectToAction("ProccedToPay"); // Return the view with the validation error
            }
            if (ViewBag.Amount > PriceOfVehicles)
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

            Sale sale = new Sale();
            sale.PaymentId = payment.Id;
            sale.UserId = CurUser.Id;
            sale.Delivered = false;
            sale.SaleDate = DateTime.Now;
            dbContext.Sales.Add(sale);
            dbContext.SaveChanges();



            //var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();

            foreach (var veh in cart)
            {
                dbContext.Entry(veh).State = EntityState.Unchanged;
                veh.SaleId = sale.Id;
                veh.Available = false;
               
                    dbContext.Update(veh);
                    dbContext.SaveChanges();
              

            }

            dbContext.SaveChanges();

            cart.Clear();
            HttpContext.Session.Set("ShoppingCart", cart);

            return RedirectToAction("Done", "Cart");
        }
    }
}
