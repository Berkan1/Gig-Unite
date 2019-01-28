using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GigUnite.Data;
using GigUnite.Models;
using Microsoft.AspNetCore.Identity;
using static GigUnite.BusinessLogic.ProfileProcessor;

namespace GigUnite.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public ProfilesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
			_userManager = userManager;
		}

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Profile.ToListAsync());
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

		public async Task<IActionResult> YourDetails()
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var data = LoadYourProfile(userId);

			if (data == 0)
			{
				CreateProfile("Unnamed", "London", new DateTime(2000, 01, 01), userId);
			}

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);
			if (profile == null)
			{
				return NotFound();
			}

			return View(profile);
		}

		private bool ProfileExists(int id)
        {
            return _context.Profile.Any(e => e.Id == id);
        }
    }
}
