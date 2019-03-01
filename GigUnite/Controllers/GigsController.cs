using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GigUnite.Data;
using GigUnite.Models;

namespace GigUnite.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController(ApplicationDbContext context)
        {
            _context = context;
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

            return View(gig);
        }

        // GET: Gigs/Create
        public IActionResult Create()
        {
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "City");
            return View();
        }

        // POST: Gigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Band,Date,Venue,Price,Location,ProfileId")] Gig gig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "City", gig.ProfileId);
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
    }
}
