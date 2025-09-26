namespace Eventera.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<AstronomicalEvent>? AstronomicalEvents { get; set; }
    }
}
