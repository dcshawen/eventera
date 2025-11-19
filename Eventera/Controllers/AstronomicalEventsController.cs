using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventera.Data;
using Eventera.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Eventera.Controllers
{
    [Authorize]
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
                .Include(a => a.Tickets)
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
        public async Task<IActionResult> Create([Bind("AstronomicalEventId,Title,Description,Location,StartDateTime,CreatedDateTime,CategoryId,ImageFile")] AstronomicalEvent astronomicalEvent)
        {
            astronomicalEvent.CreatedDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (astronomicalEvent.ImageFile != null)
                {
                    var fileUpload = astronomicalEvent.ImageFile;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
                    string savedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", filename);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(savedFilePath) ?? string.Empty);

                    using (FileStream fileStream = new FileStream(savedFilePath, FileMode.Create))
                    {
                        await fileUpload.CopyToAsync(fileStream);
                    }

                    astronomicalEvent.Filename = "/images/" + filename;
                }
                else
                {
                    // Placeholder image
                    astronomicalEvent.Filename = "/images/eclipse.jpg";
                }

                _context.Add(astronomicalEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home");
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
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", astronomicalEvent.CategoryId);
            return View(astronomicalEvent);
        }

        // POST: AstronomicalEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AstronomicalEventId,Title,Description,Location,StartDateTime,CreatedDateTime,CategoryId,ImageFile")] AstronomicalEvent astronomicalEvent)
        {
            if (id != astronomicalEvent.AstronomicalEventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Ensure the record still exists and preserve fields we shouldn't overwrite
                var existingEvent = await _context.AstronomicalEvent.AsNoTracking().FirstOrDefaultAsync(e => e.AstronomicalEventId == id);
                if (existingEvent == null)
                {
                    return NotFound();
                }

                string newFilename = existingEvent.Filename;

                // Handle new file upload if provided
                if (astronomicalEvent.ImageFile != null && astronomicalEvent.ImageFile.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(astronomicalEvent.ImageFile.FileName);
                    string savedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", filename);

                    Directory.CreateDirectory(Path.GetDirectoryName(savedFilePath) ?? string.Empty);

                    using (FileStream fileStream = new FileStream(savedFilePath, FileMode.Create))
                    {
                        await astronomicalEvent.ImageFile.CopyToAsync(fileStream);
                    }

                    newFilename = "/images/" + filename;

                    // Optionally delete the old file if it exists and is not the placeholder
                    try
                    {
                        if (!string.IsNullOrEmpty(existingEvent.Filename) && !existingEvent.Filename.EndsWith("eclipse.jpg", StringComparison.OrdinalIgnoreCase))
                        {
                            var oldFileName = Path.GetFileName(existingEvent.Filename);
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", oldFileName);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                    }
                    catch
                    {
                        // swallow any errors deleting old file
                    }
                }

                // Preserve created date and set resolved filename
                astronomicalEvent.CreatedDateTime = existingEvent.CreatedDateTime;
                astronomicalEvent.Filename = newFilename;

                try
                {
                    _context.Entry(astronomicalEvent).State = EntityState.Modified;
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

                return RedirectToAction(nameof(Index), "Home");
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", astronomicalEvent.CategoryId);
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
                // If a local file was stored in Filename, attempt to delete the file from wwwroot/images
                if (!string.IsNullOrEmpty(astronomicalEvent.Filename))
                {
                    try
                    {
                        if (!astronomicalEvent.Filename.EndsWith("eclipse.jpg", StringComparison.OrdinalIgnoreCase))
                        {
                            var fileName = Path.GetFileName(astronomicalEvent.Filename);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }
                    catch
                    {
                        // swallow exceptions to avoid failing delete operation; consider logging in real app
                    }
                }

                _context.AstronomicalEvent.Remove(astronomicalEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
        }

        private bool AstronomicalEventExists(int id)
        {
            return _context.AstronomicalEvent.Any(e => e.AstronomicalEventId == id);
        }
    }
}
