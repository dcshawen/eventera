using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.ComponentModel.DataAnnotations;

namespace Eventera.Models
{
    public class Ticket
    {
        [Display(Name = "Id")]
        public int TicketId { get; set; }
        public string PurchaserName { get; set; } = string.Empty;
        public DateTime PurchaseDateTime { get; set; }

        public int AstronomicalEventId { get; set; }
        public AstronomicalEvent? AstronomicalEvent { get; set; }
    }
}
