using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GigUnite.Data;
using GigUnite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static GigUnite.DAO.ReportDAO;

namespace GigUnite.Controllers
{
	[Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public ReportsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Reports
		public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Report.Include(r => r.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

		[AllowAnonymous]
		// GET: Reports/Create
		public async Task<IActionResult> Create(int profileId)
        {
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profile2 = await _context.Profile
				.FirstOrDefaultAsync(m => m.Id == profileId);

			ViewBag.ReportBy = profile.Displayname;
			ViewBag.ProfileId = profileId;
			ViewBag.ProfileName = profile2.Displayname;

			return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportBy,Reason,ProfileId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
				return RedirectToAction("Dashboard", "Home");
			}
			return RedirectToAction("Dashboard", "Home");
		}

		public async Task<IActionResult> Deactivate(int profileId)
		{
			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.Id == profileId);

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == profile.UserId);

			DeleteUser(profileId, user.Id);

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Dismiss(int reportId)
		{
			DeleteReport(reportId);

			return RedirectToAction(nameof(Index));
		}

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.Id == id);
        }
    }
}
