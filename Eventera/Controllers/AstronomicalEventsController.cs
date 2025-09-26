using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventera.Data;
using Eventera.Models;

namespace Eventera.Controllers
{
    public class AstronomicalEventsController : Controller
    {
        private readonly EventeraContext _context;

        public AstronomicalEventsController(EventeraContext context)
        {
            _context = context;
        }

        // GET: AstronomicalEvents
        public async Task<IActionResult> Index()
        {
            var eventeraContext = _context.AstronomicalEvent.Include(a => a.Category);
            return View(await eventeraContext.ToListAsync());
        }

        // GET: AstronomicalEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var astronomicalEvent = await _context.AstronomicalEvent
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.AstronomicalEventId == id);
            if (astronomicalEvent == null)
            {
                return NotFound();
            }

            return View(astronomicalEvent);
        }

        // GET: AstronomicalEvents/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title");
            return View();
        }

        // POST: AstronomicalEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AstronomicalEventId,Title,Description,Location,StartDateTime,CreatedDateTime,CategoryId")] AstronomicalEvent astronomicalEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(astronomicalEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", astronomicalEvent.CategoryId);
            return View(astronomicalEvent);
        }

        // GET: AstronomicalEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var astronomicalEvent = await _context.AstronomicalEvent.FindAsync(id);
            if (astronomicalEvent == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", astronomicalEvent.CategoryId);
            return View(astronomicalEvent);
        }

        // POST: AstronomicalEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AstronomicalEventId,Title,Description,Location,StartDateTime,CreatedDateTime,CategoryId")] AstronomicalEvent astronomicalEvent)
        {
            if (id != astronomicalEvent.AstronomicalEventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(astronomicalEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AstronomicalEventExists(astronomicalEvent.AstronomicalEventId))
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
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", astronomicalEvent.CategoryId);
            return View(astronomicalEvent);
        }

        // GET: AstronomicalEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var astronomicalEvent = await _context.AstronomicalEvent
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.AstronomicalEventId == id);
            if (astronomicalEvent == null)
            {
                return NotFound();
            }

            return View(astronomicalEvent);
        }

        // POST: AstronomicalEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var astronomicalEvent = await _context.AstronomicalEvent.FindAsync(id);
            if (astronomicalEvent != null)
            {
                _context.AstronomicalEvent.Remove(astronomicalEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AstronomicalEventExists(int id)
        {
            return _context.AstronomicalEvent.Any(e => e.AstronomicalEventId == id);
        }
    }
}
