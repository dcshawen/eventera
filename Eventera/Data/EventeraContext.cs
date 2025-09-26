using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eventera.Models;

namespace Eventera.Data
{
    public class EventeraContext : DbContext
    {
        public EventeraContext (DbContextOptions<EventeraContext> options)
            : base(options)
        {
        }

        public DbSet<Eventera.Models.AstronomicalEvent> AstronomicalEvent { get; set; } = default!;
        public DbSet<Eventera.Models.Category> Category { get; set; } = default!;
    }
}
