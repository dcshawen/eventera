using System.Diagnostics;
using Eventera.Data;
using Eventera.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eventera.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventeraContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, EventeraContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var upcomingEvents = await _context.AstronomicalEvent
                .Where(e => e.StartDateTime >= DateTime.Now)
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();
            return View(upcomingEvents);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
