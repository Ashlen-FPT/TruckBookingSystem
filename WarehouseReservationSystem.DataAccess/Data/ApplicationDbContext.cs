using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseReservationSystem.Models;
using WarehouseReservationSystem.Models.Fruit;
using WarehouseReservationSystem.Models.GC;

namespace WarehouseReservationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Exporter> Exporter { get; set; }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<Transporter> Transporter { get; set; }

        public DbSet<Warehouse> Warehouse { get; set; }

        public DbSet<MarketType> MarketTypes { get; set; }

        public DbSet<Slots> Slots { get; set; }

        public DbSet<FileSeq> Sequence { get; set; }

        public DbSet<Vessel> Vessel { get; set; }
        
        public DbSet<GCBooking> GCBooking { get; set; }

        public DbSet<GCSlots> GCSlots { get; set; }
    }
}
