using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Autos.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Autos.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+$",ErrorMessage ="Email should contain @ and .")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            [RegularExpression(@"^01\d{9}$", ErrorMessage = "Please enter 11 digits for Your PhoneNumber starting wuth 01")]
            public string PhoneNumber { get; set; }

            [Required]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "No numbers allowed in First Name")]
            public string Firstname { get; set; }
            [Required]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "No numbers allowed in  Last Name")]
            public string Lastname { get; set; }

            public string? Image { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile img_file = null,string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string ? fileExtension =null;
            if (img_file != null)
                fileExtension = Path.GetExtension(img_file.FileName)?.ToLower();

            if (ModelState.IsValid&&( (fileExtension != null&&allowedExtensions.Contains(fileExtension))||fileExtension==null))
            {
                
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
                else
                {
                    Input.Image = "default.png";
                }
                var user = new User
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    Firstname = Input.Firstname,
                    Lastname = Input.Lastname,
                    Image = Input.Image
                    
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if(_signInManager.IsSignedIn(User)&& User.IsInRole("SuperAdmin"))
                        await _userManager.AddToRoleAsync(user, "Admin");
                    else 
                    await _userManager.AddToRoleAsync(user, "User");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (_signInManager.IsSignedIn(User) && User.IsInRole("SuperAdmin"))
                        {
                            return Redirect("/Admin/ListOfAdmins");

                        }
                        else
                        {

                           // await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToPage("/Account/Login", new { area = "Identity" });
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else if ((fileExtension != null && !allowedExtensions.Contains(fileExtension)))
            ModelState.AddModelError("ProfileImageFile", "Please upload only images with the following extensions: .jpg, .jpeg, .png");

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
