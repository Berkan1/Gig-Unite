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
using static GigUnite.DAO.ProfileDAO;
using Microsoft.AspNetCore.Authorization;

namespace GigUnite.Controllers
{
	[Authorize]
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

		public async Task<IActionResult> MyProfile()
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);
			if (profile == null)
			{
				return NotFound();
			}

			return View(profile);
		}

		// GET: Profiles/EditProfile/
		public async Task<IActionResult> Edit()
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);
			if (profile == null)
			{
				return NotFound();
			}

			var genres = from m in _context.Genre
						 select m.Name;

			ViewBag.Genres = genres;

			return View(profile);
		}

		// POST: Profiles/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([Bind("Id,Displayname,City,Dob,Bio,UserId")] Profile profile, string ProfileGenre)
		{
			if (ProfileGenre == "Rock")
			{
				return RedirectToAction(nameof(Index));
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(profile);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProfileExists(profile.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(MyProfile));
			}
			return View(profile);
		}

		private bool ProfileExists(int id)
        {
            return _context.Profile.Any(e => e.Id == id);
        }
    }
}
