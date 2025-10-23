using System.Diagnostics;
using Eventera.Data;
using Eventera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eventera.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EventeraContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, EventeraContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: AstronomicalEvents
        public async Task<IActionResult> Index()
        {
            //var eventeraContext = _context.AstronomicalEvent.Include(a => a.Category);
            //return View(await eventeraContext.ToListAsync());
            var events = await _context.AstronomicalEvent
                .Include(e => e.Category)
                //.OrderBy(e => e.StartDateTime)
                .ToListAsync();
            return View(events);
        }


        // GET: AstronomicalEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.AstronomicalEvent
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.AstronomicalEventId == id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
