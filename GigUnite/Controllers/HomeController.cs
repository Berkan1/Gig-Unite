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
using static GigUnite.DAO.HomeDAO;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace GigUnite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Dashboard");
			}
			else
			{
				var gigs = from m in _context.Gig 
						   select m;

				var recentGig = new List<Gig>();

				foreach (var gig in gigs)
				{
					recentGig.Clear();
					recentGig.Add(gig);
				}

				return View(gigs);
			}
		}

		[Authorize]
		public async Task<IActionResult> Dashboard()
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profileId = profile.Id;

			var profileGenres = from m in _context.ProfileGenre
						 where m.ProfileId == profileId
						 select m.Genre.Name;

			List<string> genres = new List<string>();

			foreach (string genre in profileGenres)
			{
				genres.Add(genre);
			}

			var data = LoadRecommendedGigs(profileId, genres);
			var data2 = LoadInterestedGigs(profileId);

			List<int> ranking = (from m in data
								 group m by m into rank
								 orderby rank.Count() descending
								 select rank.Key).ToList();

			List<Gig> recommendedGigs = new List<Gig>();
			List<Gig> interestedGigs = new List<Gig>();

			foreach (var row in ranking)
			{
				var gig = await _context.Gig
				.FirstOrDefaultAsync(m => m.Id == row);

				recommendedGigs.Add(gig);
			}

			foreach (var gigId in data2)
			{
				var gig = await _context.Gig
				.FirstOrDefaultAsync(m => m.Id == gigId);

				interestedGigs.Add(gig);
			}

			DashboardModel dashboardModel = new DashboardModel
			{
				RecommendedGigs = recommendedGigs,
				InterestedGigs = interestedGigs
			};

			return View(dashboardModel);
		}

		public IActionResult Terms()
		{
			string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

			ViewBag.Url = url;
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}