﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Eventera.Models
{
    public class AstronomicalEvent
    {
        [Display(Name = "Id")]
        public int AstronomicalEventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        [Display(Name = "Start Date")]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedDateTime { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [NotMapped]
        [Display(Name = "Image File")]
        public IFormFile? ImageFile { get; set; }

        public AstronomicalEvent()
        {
            CreatedDateTime = DateTime.Now;
        }
    }
}
