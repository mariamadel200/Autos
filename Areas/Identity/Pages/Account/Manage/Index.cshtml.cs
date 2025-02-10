using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Autos.Models;
using Microsoft.Extensions.Hosting;

namespace Autos.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }




        public class InputModel
        {
           
          
            [Phone]
            [Display(Name = "Phone Number")]
            [RegularExpression("[0-9]{11}", ErrorMessage = "Please enter 11 digits for Your PhoneNumber")]
            public string PhoneNumber { get; set; }

         
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "No numbers allowed in First Name")]
            public string Firstname { get; set; }
            

            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "No numbers allowed in  Last Name")]
            public string Lastname { get; set; }

            public string? Image { get; set; }

        }






        private async Task LoadAsync(User user)
        {
          
            var EndUser = await _userManager.GetUserAsync(User);

            Input = new InputModel
            {
              Firstname= EndUser.Firstname,
              Lastname= EndUser.Lastname,
              PhoneNumber= EndUser.PhoneNumber,
              Image= EndUser.Image

            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile img_file = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string? fileExtension = null;
            if (img_file != null)
                fileExtension = Path.GetExtension(img_file.FileName)?.ToLower();


            if (!ModelState.IsValid||((fileExtension != null && !allowedExtensions.Contains(fileExtension))))
            {
                ModelState.AddModelError("ProfileImageFile", "Please upload only images with the following extensions: .jpg, .jpeg, .png");
                await LoadAsync(user);
                return Page();
            }
            string path = Path.Combine(_environment.WebRootPath, "assets/images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (img_file != null)
            {
                path = Path.Combine(path, img_file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await img_file.CopyToAsync(stream);
                    // ViewBag.Message = string.Format("<b>{0}</b> uploaded.</br>", img_file.FileName.ToString());
                }
                Input.Image = img_file.FileName;
            }
            else Input.Image = user.Image;
            
           // Console.WriteLine(Input.Image);
                user.Firstname = Input.Firstname;
                user.Lastname = Input.Lastname;
                user.PhoneNumber = Input.PhoneNumber;
                user.Image = Input.Image;
                var updateResult = await _userManager.UpdateAsync(user);
               if (!updateResult.Succeeded)
            {
                
                StatusMessage = "Unexpected error";
                return RedirectToPage();
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
