using System;
using System.Linq;
using System.Threading.Tasks;
using Autos.Data;
using Autos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autos.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ManageVehicleController : Controller
    {
        private readonly AutoDbContext DbContext;

        public ManageVehicleController(AutoDbContext context)
        {
            DbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListOfVehiclesForSale()
        {
            var vehicles = DbContext.VehiclesForSale.ToList();
            return View(vehicles);
        }

        public IActionResult ListOfVehiclesForRent()
        {
            var vehicles = DbContext.VehiclesForRent.ToList();
            return View(vehicles);
        }

        public IActionResult AddForSale()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddForSale(VehicleForSale newVehicle)
        {
            newVehicle.Available = true;
            DbContext.VehiclesForSale.Add(newVehicle);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("ListOfVehiclesForSale");
        }

        public IActionResult AddForRent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddForRent(VehicleForRent newVehicle)
        {
            DbContext.VehiclesForRent.Add(newVehicle);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("ListOfVehiclesForRent");
        }

        public IActionResult EditVehicleForSale(int id)
        {
            var vehicle = DbContext.VehiclesForSale.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> EditVehicleForSale(VehicleForSale updatedVehicle)
        {
            if (ModelState.IsValid)
            {
                DbContext.Update(updatedVehicle);
                await DbContext.SaveChangesAsync();
                return RedirectToAction("ListOfVehiclesForSale");
            }
            return RedirectToAction();
        }

        public IActionResult EditVehicleForRent(int id)
        {
            var vehicle = DbContext.VehiclesForRent.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> EditVehicleForRent(VehicleForRent updatedVehicle)
        {
            if (ModelState.IsValid)
            {
                DbContext.Update(updatedVehicle);
                await DbContext.SaveChangesAsync();
                return RedirectToAction("ListOfVehiclesForRent");
            }
            return RedirectToAction();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveForSale(int id)
        {
            var vehicleToRemove = await DbContext.VehiclesForSale.FindAsync(id);
            if (vehicleToRemove == null)
            {
                return NotFound();
            }
            DbContext.VehiclesForSale.Remove(vehicleToRemove);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("ListOfVehiclesForSale");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveForRent(int id)
        {
            var vehicleToRemove = await DbContext.VehiclesForRent.FindAsync(id);
            if (vehicleToRemove == null)
            {
                return NotFound();
            }
            DbContext.VehiclesForRent.Remove(vehicleToRemove);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("ListOfVehiclesForRent");
        }

        public IActionResult Sold(string filter)
        {
            var sales = DbContext.Sales
                .Include(s => s.User)
                .Include(s => s.Payment)
                .AsQueryable();

            switch (filter?.ToLower())
            {
                case "day":
                    sales = sales.Where(s => s.SaleDate >= DateTime.Now.AddDays(-1));
                    break;
                case "week":
                    sales = sales.Where(s => s.SaleDate >= DateTime.Now.AddDays(-7));
                    break;
                case "month":
                    sales = sales.Where(s => s.SaleDate >= DateTime.Now.AddMonths(-1));
                    break;
                case "year":
                    sales = sales.Where(s => s.SaleDate >= DateTime.Now.AddYears(-1));
                    break;
                default:
                    break;
            }

            return View(sales.ToList());
        }

        public IActionResult Rented(string filter)
        {
            var rents = DbContext.Rentals
                .Include(r => r.User)
                .Include(r => r.Payment)
                .AsQueryable();

            switch (filter?.ToLower())
            {
                case "day":
                    rents = rents.Where(r => r.StartDate >= DateTime.Now.AddDays(-1));
                    break;
                case "week":
                    rents = rents.Where(r => r.StartDate >= DateTime.Now.AddDays(-7));
                    break;
                case "month":
                    rents = rents.Where(r => r.StartDate >= DateTime.Now.AddMonths(-1));
                    break;
                case "year":
                    rents = rents.Where(r => r.StartDate >= DateTime.Now.AddYears(-1));
                    break;
                default:
                    break;
            }

            return View(rents.ToList());
        }
        public async Task<IActionResult> DeliveredforSale(int id)
        {
            var sale = await DbContext.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            sale.Delivered = true;
            await DbContext.SaveChangesAsync();
            return RedirectToAction("Sold");
        }
        public async Task<IActionResult> DeliveredforRents(int id)
        {
            var rent = await DbContext.Rentals.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            rent.Delivered = true;
            await DbContext.SaveChangesAsync();
            return RedirectToAction("Rented");
        }
    }
}
