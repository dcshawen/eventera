using Eventera.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eventera.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            // Test events. Remove this after CRUD Implementation
            List<Event> events = new List<Event>
            {
                new Event {
                    EventId = 1,
                    Title = "Music Concert",
                    Description = "A live music concert featuring popular bands.",
                    Location = "City Arena",
                    EventDate = new DateTime(2025, 10, 15),
                    EventTime = new DateTime(2025, 10, 15, 19, 0, 0),
                    CreatedDateTime = DateTime.Now,
                    CategoryId = 1,
                    Category = new Category {
                        CategoryId = 1,
                        Title = "Entertainment",
                        Description = "Events related to music, movies, and shows."
                    }
                },
                new Event {
                    EventId = 2,
                    Title = "Tech Conference",
                    Description = "Annual conference discussing the latest in technology.",
                    Location = "Convention Center",
                    EventDate = new DateTime(2025, 11, 5),
                    EventTime = new DateTime(2025, 11, 5, 9, 0, 0),
                    CreatedDateTime = DateTime.Now,
                    CategoryId = 2,
                    Category = new Category {
                        CategoryId = 2,
                        Title = "Technology",
                        Description = "Events related to technology and innovation."
                    }
                }
            };

            return View(events);
        }
    }
}
