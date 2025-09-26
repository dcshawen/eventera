using System.ComponentModel;

namespace Eventera.Models
{
    public class AstronomicalEvent
    {
        public int AstronomicalEventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public AstronomicalEvent()
        {
            CreatedDateTime = DateTime.Now;
        }
    }
}
