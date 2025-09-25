using Eventera.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eventera.Controllers
{
    public class TournamentsController : Controller
    {
        public IActionResult Index()
        {
            // Test events. Remove this after CRUD Implementation
            List<Tournament> tournaments = new List<Tournament>
            {
                new Tournament {
                    TournamentId = 1,
                    Title = "Music Concert",
                    Description = "A live music concert featuring popular bands.",
                    Location = "City Arena",
                    StartDateTime = new DateTime(2024, 9, 15, 19, 0, 0),
                    CreatedDateTime = DateTime.Now,
                    CategoryId = 1,
                    Category = new Category {
                        CategoryId = 1,
                        Title = "Melee",
                        Description = "Tournaments related to music, movies, and shows."
                    }
                },
                new Tournament {
                    TournamentId = 2,
                    Title = "Tech Conference",
                    Description = "Annual conference discussing the latest in technology.",
                    Location = "Convention Center",
                    StartDateTime = new DateTime(2024, 10, 5, 9, 0, 0),
                    CreatedDateTime = DateTime.Now,
                    CategoryId = 2,
                    Category = new Category {
                        CategoryId = 2,
                        Title = "Archery",
                        Description = "Tournaments related to technology and innovation."
                    }
                }
            };

            return View(tournaments);
        }
    }
}
