using Autos.Data;
using Autos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Autos.Controllers
{
    [Authorize(Roles = "User")]
    public class SearchController : Controller
    {
        private readonly AutoDbContext dbContext;
        private readonly UserManager<User> _userManager;
        public SearchController(AutoDbContext Context, UserManager<User> userManager)
        {
            dbContext = Context;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Search(VehicleForSale v)
        {
            IQueryable<VehicleForSale> query = dbContext.VehiclesForSale;

            query = query.Where(Vehicle => Vehicle.Available == true);

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

            if (v.Price != 0.0)
            {
                query = query.Where(veh => veh.Price <= v.Price);
            }
           
            var cart = HttpContext.Session.Get<List<VehicleForSale>>("ShoppingCart") ?? new List<VehicleForSale>();
            var cartid = cart.Select(c => c.Id).ToHashSet();
            var results = query.Where(r => !cartid.Contains(r.Id)).ToList();

            return View("Result", results);
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


    }
}