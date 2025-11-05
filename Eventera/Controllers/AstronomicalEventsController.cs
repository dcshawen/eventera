using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventera.Data;
using Eventera.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Eventera.Controllers
{
    [Authorize]
    public class AstronomicalEventsController : Controller
    {
        private readonly EventeraContext _context;

        private readonly BlobContainerClient _containerClient;
        private readonly IConfiguration _configuration;

        public AstronomicalEventsController(IConfiguration configuration, EventeraContext context)
        {
            _context = context;
            _configuration = configuration;

            var connectionString = _configuration["AzureStorage"];
            if (connectionString.IsNullOrEmpty())
            {
                connectionString = _configuration.GetConnectionString("AzureStorage");
            }
            var containerName = "nscc0190983blobcontainer";
            _containerClient = new BlobContainerClient(connectionString, containerName);
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
        public async Task<IActionResult> Create([Bind("AstronomicalEventId,Title,Description,Location,StartDateTime,CreatedDateTime,CategoryId,ImageFile")] AstronomicalEvent astronomicalEvent)
        {
            astronomicalEvent.CreatedDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (astronomicalEvent.ImageFile != null)
                {
                    //string filename = Guid.NewGuid().ToString() + Path.GetExtension(astronomicalEvent.ImageFile.FileName);
                    //astronomicalEvent.Filename = filename;
                    //string savedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", filename);

                    //using (FileStream fileStream = new FileStream(savedFilePath, FileMode.Create))
                    //{
                    //    await astronomicalEvent.ImageFile.CopyToAsync(fileStream);
                    //}

                    IFormFile fileUpload = astronomicalEvent.ImageFile;
                    string blobName = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;
                    var blobClient = _containerClient.GetBlobClient(blobName);
                    
                    using (var stream = fileUpload.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileUpload.ContentType });
                    }

                    string blobUrl = blobClient.Uri.ToString();

                    astronomicalEvent.Filename = blobUrl;
                } else
                {
                    var blobClient = _containerClient.GetBlobClient("eclipse.jpg");
                    string blobUrl = blobClient.Uri.ToString();
                    astronomicalEvent.Filename = blobUrl;
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
        public async Task<IActionResult> Edit(int id, [Bind("AstronomicalEventId,Title,Description,Location,Filename,StartDateTime,CreatedDateTime,CategoryId,ImageFile")] AstronomicalEvent astronomicalEvent)
        {
            if (id != astronomicalEvent.AstronomicalEventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.AstronomicalEvent.AsNoTracking().FirstOrDefaultAsync(e => e.AstronomicalEventId == id);
                    if (existingEvent == null)
                    {
                        return NotFound();
                    }

                    string oldFilename = existingEvent.Filename;
                    string newFilename = oldFilename;

                    if (astronomicalEvent.ImageFile != null && astronomicalEvent.ImageFile.Length > 0)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(astronomicalEvent.ImageFile.FileName);
                        string savedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", filename);

                        using (FileStream fileStream = new FileStream(savedFilePath, FileMode.Create))
                        {
                            await astronomicalEvent.ImageFile.CopyToAsync(fileStream);
                        }

                        newFilename = filename;

                        if (!string.IsNullOrEmpty(oldFilename))
                        {
                            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", oldFilename);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }

                    astronomicalEvent.Filename = newFilename;
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
                // If a blob URL was stored in Filename, attempt to delete the blob from storage
                if (!string.IsNullOrEmpty(astronomicalEvent.Filename))
                {
                    try
                    {
                        var uri = new Uri(astronomicalEvent.Filename);
                        // Blob URL path typically looks like: /containerName/blobName
                        var segments = uri.Segments;
                        if (segments.Length >= 1)
                        {
                            // Last segment is the blob name (handles simple blob names)
                            var blobName = segments[^1].Trim('/');
                            if (!string.IsNullOrEmpty(blobName))
                            {
                                var blobClient = _containerClient.GetBlobClient(blobName);
                                await blobClient.DeleteIfExistsAsync();
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
