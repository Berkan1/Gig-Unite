﻿using System;
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
            var applicationDbContext = _context.Gig.Include(g => g.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Gigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gig = await _context.Gig
                .Include(g => g.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gig == null)
            {
                return NotFound();
            }

			ViewBag.InterestLevel = "N/A";

			if (_context.Interest.Any(m => m.EventId == id))
			{
				var interestLevel = await _context.Interest
				.FirstOrDefaultAsync(m => m.EventId == id);

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
        public async Task<IActionResult> Create([Bind("Id,Band,Date,Venue,Price,Location,ProfileId")] Gig gig, List<string> genres)
        {
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

				var recipients = GetEmailList(gig.Band, genres);

				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("GigUnite", "gigunite@hotmail.com"));
				foreach (var recipient in recipients)
				{
					message.Bcc.Add(new MailboxAddress(recipient));
				}
				message.Subject = "New Gig Advert for " + gig.Band;
				message.Body = new TextPart("plain")
				{
					Text = "Hi! A new gig advert was posted by " + profile.Displayname + " with the following details: \r\n \r\n Artist/Band: " + gig.Band +
						   "\r\n Date: " + gig.Date.ToString("dd/MM/yyyy") + "\r\n Venue: " + gig.Venue + "\r\n Price: " + gig.Price + "\r\n Genres: " + gigGenres
				};
				using (var client = new SmtpClient())
				{
					client.Connect("smtp.live.com", 587, false);
					client.Authenticate("gigunite@hotmail.com", "PASSWORD");
					client.Send(message);
					client.Disconnect(true);
				}

				return RedirectToAction(nameof(Index));
            }
            return View(gig);
        }

        // GET: Gigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gig = await _context.Gig.FindAsync(id);
            if (gig == null)
            {
                return NotFound();
            }
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "City", gig.ProfileId);
            return View(gig);
        }

        // POST: Gigs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Band,Date,Venue,Price,Location,ProfileId")] Gig gig)
        {
            if (id != gig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GigExists(gig.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "City", gig.ProfileId);
            return View(gig);
        }

        // GET: Gigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gig = await _context.Gig
                .Include(g => g.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gig == null)
            {
                return NotFound();
            }

            return View(gig);
        }

        // POST: Gigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gig = await _context.Gig.FindAsync(id);
            _context.Gig.Remove(gig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
	}
}