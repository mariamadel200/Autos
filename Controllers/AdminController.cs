using Autos.Data;
using Autos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Autos.Controllers
{
   //[Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddAdmin() 
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(User model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Firstname=model.Firstname,Lastname=model.Lastname,UserName = model.UserName, Email = model.Email,PhoneNumber=model.PhoneNumber };

                var result = await _userManager.CreateAsync(user, model.PasswordHash);
                await _userManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    // Add user to "Admin" role
                    await _userManager.AddToRoleAsync(user, "Admin");

                    return RedirectToAction("ListOfAdmins");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ListOfAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return View(admins);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (!result.Succeeded)
            {
                // Handle error
            }

            return RedirectToAction("ListOfAdmins");
        }
    }
}
