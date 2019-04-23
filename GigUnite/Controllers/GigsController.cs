using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GigUnite.Data;
using GigUnite.Models;
using static GigUnite.DAO.GigDAO;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;

namespace GigUnite.Controllers
{
	[Authorize]
	public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public GigsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
			_userManager = userManager;
		}

        // GET: Gigs
        public async Task<IActionResult> Index()
        {
			var gigs = from m in _context.Gig.Include(g => g.Profile)
					   select m;

			return View(await gigs.ToListAsync());
		}

        // GET: Gigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profileId = profile.Id;

			var gig = await _context.Gig
                .Include(g => g.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gig == null)
            {
                return NotFound();
            }

			ViewBag.InterestLevel = "N/A";

			if (_context.Interest.Any(m => m.EventId == id && m.UserId == profile.Id))
			{
				var interestLevel = await _context.Interest
				.FirstOrDefaultAsync(m => m.EventId == id && m.UserId == profile.Id);

				ViewBag.InterestLevel = interestLevel.Status;
			}

			var gigGenres = from m in _context.GigGenre
						   where m.GigId == gig.Id
						   select m.Genre.Name;

			ViewBag.GigGenres = gigGenres;

			var commentMessages = from m in _context.Comment
								  where m.GigId == gig.Id
								  select m.Message;

			var messages = new List<string>();

			foreach (string comment in commentMessages)
			{
				messages.Add(comment);
			}

			ViewBag.Messages = messages;

			var commentTimePosted = from m in _context.Comment
									where m.GigId == gig.Id
									select m.TimePosted;

			var timesPosted = new List<DateTime>();

			foreach (DateTime times in commentTimePosted)
			{
				timesPosted.Add(times);
			}

			ViewBag.TimesPosted = timesPosted;

			var commentUser = from m in _context.Comment
									where m.GigId == gig.Id
									select m.UserId;

			var profiles = new List<string>();

			foreach (int user in commentUser)
			{
				var profileUser = from m in _context.Profile
								  where m.Id == user
								  select m.Displayname;

				string first = profileUser.First();

				profiles.Add(first);
			}

			ViewBag.Profiles = profiles;

			var peopleGoing = new List<string>();
			var peopleInterested = new List<string>();

			var idGoing = from m in _context.Interest
							  where m.EventId == gig.Id
							  && m.Status == "Going"
							  select m.UserId;

			var idInterested = from m in _context.Interest
							   where m.EventId == gig.Id
							   && m.Status == "Interested"
							   select m.UserId;

			foreach (int person in idGoing)
			{
				var personGoing = await _context.Profile
				.FirstOrDefaultAsync(m => m.Id == person);

				peopleGoing.Add(personGoing.Displayname);
			}

			foreach (int person in idInterested)
			{
				var personGoing = await _context.Profile
				.FirstOrDefaultAsync(m => m.Id == person);

				peopleInterested.Add(personGoing.Displayname);
			}

			ViewBag.Going = peopleGoing;
			ViewBag.Interested = peopleInterested;

			gig.Views += 1;

			_context.SaveChanges();

			return View(gig);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Details(string message, int gigId)
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profileId = profile.Id;

			string timeNow = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

			AddComment(profileId, gigId, timeNow, message);

			return RedirectToAction("Details", new { id = gigId });
		}

		// GET: Gigs/Create
		public async Task<IActionResult> Create()
        {
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profileId = profile.Id;

			var genres = from m in _context.Genre
						 select m.Name;

			ViewBag.Genres = genres;
			ViewBag.ProfileId = profileId;

			return View();
        }

        // POST: Gigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Band,Date,Venue,Price,TicketLink,Views,ProfileId")] Gig gig, List<string> genres)
        {

			if (gig.Date <= DateTime.Now)
			{
				TempData["DateError"] = "Gig dates must be in the future";
				return RedirectToAction(nameof(Create));
			}

			if (ModelState.IsValid)
            {
                _context.Add(gig);
                await _context.SaveChangesAsync();

				string gigGenres = "| ";

				foreach (var genre in genres)
				{
					AddGenresToGig(gig.Id, genre);
					gigGenres += genre + " | ";
				}

				var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.Id == gig.ProfileId);

				var currentUser = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == profile.UserId);

				var recipients = GetEmailList(gig.Band, genres);
				recipients.Remove(currentUser.Email);

				if(recipients.Count > 0)
				{
					string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Identity/Account/Manage";

					var message = new MimeMessage();
					message.From.Add(new MailboxAddress("GigUnite", "gigunite@hotmail.com"));
					foreach (var recipient in recipients)
					{
						message.Bcc.Add(new MailboxAddress(recipient));
					}
					message.Subject = "New Gig Advert for " + gig.Band;

					var bodyBuilder = new BodyBuilder();
					bodyBuilder.HtmlBody = "Hi! A new gig advert was posted by " + profile.Displayname + " with the following details: <br /><br /> Artist/Band: " + gig.Band +
							   "<br /> Date: " + gig.Date.ToString("dd/MM/yyyy") + "<br /> Venue: " + gig.Venue + "<br /> Price: £" + gig.Price + "<br /> Genres: " + gigGenres +
							   "<br /><br /> <a href='" + url + "'>Unsubscribe</a>";

					message.Body = bodyBuilder.ToMessageBody();
					using (var client = new SmtpClient())
					{
						client.Connect("smtp.live.com", 587, false);
						client.Authenticate("gigunite@hotmail.com", "PASSWORD");
						client.Send(message);
						client.Disconnect(true);
					}
				}

				return RedirectToAction(nameof(Index));
            }
            return View(gig);
        }

        private bool GigExists(int id)
        {
            return _context.Gig.Any(e => e.Id == id);
        }

		public async Task<IActionResult> AddInterest(int gigId, string level)
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profileId = profile.Id;

			SetInterest(gigId, profileId, level);

			return RedirectToAction("Details", new { id = gigId });
		}

		public async Task<IActionResult> RemoveInterest(int gigId)
		{
			string userId = _userManager.GetUserId(HttpContext.User);

			var profile = await _context.Profile
				.FirstOrDefaultAsync(m => m.UserId == userId);

			var profileId = profile.Id;

			DeleteInterest(gigId, profileId);

			return RedirectToAction("Details", new { id = gigId });
		}
	}
}