using Microsoft.AspNetCore.Mvc;
using Autos.Models;
using Microsoft.EntityFrameworkCore;
using Autos.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Autos.Controllers
{
    [Authorize(Roles = "User")]
    public class RentalController : Controller
    {
        private readonly AutoDbContext dbContext;
        private readonly UserManager<User> _userManager;
        public RentalController(AutoDbContext Context, UserManager<User> userManager)
        {
            dbContext = Context;
            _userManager = userManager;
        }
        public IActionResult Period()
        {
            if (HttpContext.Session.GetString("StartDate") != null&& HttpContext.Session.GetString("StartDate") != "")
            {
                return RedirectToAction("Search");
            }
            return View();
        }

        public IActionResult ChangePeriod()
        {

            return View("Period");
        }
        [HttpPost]
        public IActionResult Period(DateTime start,DateTime end)
        {

          
           int h = DateTime.Compare(start, end);
            if (h > 0)
            {
                ModelState.AddModelError("start", "The Start must be earlier than or equal to End\n");
               // Console.WriteLine("\n");
            }
            DateTime today = DateTime.Today;
            int k = DateTime.Compare(today, start);
            if (k > 0)
            {
                ModelState.AddModelError("start", "The Date must be later than today");

               // Console.WriteLine("The Date must be earlier than or equal to today\n");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                HttpContext.Session.SetString("StartDate", start.ToString());
                HttpContext.Session.SetString("EndDate", end.ToString());

                return RedirectToAction("Search");
            }
        }
        [HttpGet]
        public IActionResult Search()
        {
            var brands = dbContext.VehiclesForSale
                                  .Select(b => b.Brand)
                                  .Distinct()
                                  .ToList();

            ViewBag.Brands = new SelectList(brands);

            var colors = dbContext.VehiclesForSale
                                  .Select(c => c.Color)
                                  .Distinct()
                                  .ToList();

            ViewBag.Colors = new SelectList(colors);

            return View();
        }
        [HttpPost]
        public IActionResult Search(VehicleForRent v)
        {
            IQueryable<VehicleForRent> query = dbContext.VehiclesForRent;

           
            // Check type
            if (!string.IsNullOrEmpty(v.Type))
            {
                query = query.Where(veh => veh.Type == v.Type);
            }

            // Check brand
            if (!string.IsNullOrEmpty(v.Brand))
            {
                query = query.Where(veh => veh.Brand == v.Brand);
            }

            // Check model
            if (!string.IsNullOrEmpty(v.Model))
            {
                query = query.Where(veh => veh.Model == v.Model);
            }

            // Check year

            if (v.Year != null)
            {
                int yearValue;
                if (int.TryParse(v.Year, out yearValue))
                {
                    query = query.Where(veh => Convert.ToInt32(veh.Year) >= yearValue);
                }
            }

            // Check color
            if (!string.IsNullOrEmpty(v.Color))
            {
                query = query.Where(veh => veh.Color == v.Color);
            }

            if (v.PricePerDay != 0.0)
            {
                query = query.Where(veh => veh.PricePerDay <= v.PricePerDay);
            }
            query = query.Include(q => q.Rentals);
            DateTime Start, End;
            DateTime.TryParse(HttpContext.Session.GetString("StartDate"), out Start);
            DateTime.TryParse(HttpContext.Session.GetString("EndDate"), out End);


            query = query.Where(q => !q.Rentals.Any(re => re.StartDate <= End && re.EndDate >= Start));

            var cart = HttpContext.Session.Get<List<VehicleForRent>>("RentCart") ?? new List<VehicleForRent>();
            var cartid=cart.Select(c => c.Id).ToHashSet();
            var result=query.Where(r => !cartid.Contains(r.Id)).ToList();
           
           
            return View("Result", result);
        }
       
    }
}
