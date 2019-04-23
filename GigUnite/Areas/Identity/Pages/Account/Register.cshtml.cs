using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static GigUnite.DAO.ProfileDAO;

namespace GigUnite.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

		public async Task<IActionResult> OnPostAsync(string returnUrl = null, string displayname = null, string dob = null, string terms = null, string optin = null)
        {
			if (terms != "on")
			{
				ViewData["TermsError"] = "You need to agree to the Terms and Conditions and Privacy Policy to create an account";
				return Page();
			}

			if (optin != "on")
			{
				optin = "off";
			}

			if (displayname == null || displayname == "")
			{
				ViewData["Error"] = "Please enter a display name";
				return Page();
			}

			if (CheckNameAvailability(displayname) == 1)
			{
				ViewData["Error"] = "This display name is already taken";
				return Page();
			}

			string[] dates = dob.Split('-');
			var newdob = new DateTime(int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]));

			if (newdob > DateTime.Now)
			{
				ViewData["DateError"] = "This is not a valid date of birth";
				return Page();
			}

			int years = DateTime.Now.Year - newdob.Year;
			
			if (DateTime.Now.Month < newdob.Month)
			{
				years = years - 1;
			}
			if (DateTime.Now.Month == newdob.Month && DateTime.Now.Day < newdob.Day)
			{
				years = years - 1;
			}

			if (years < 18)
			{
				ViewData["DateError"] = "You must be at least 18 years old to create an account";
				return Page();
			}

			returnUrl = returnUrl ?? Url.Content("~/Profiles/Edit");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

					CreateProfile(displayname, newdob, user.Id, optin);

					await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
