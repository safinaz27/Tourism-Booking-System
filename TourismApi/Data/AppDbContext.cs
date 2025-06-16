using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TourismApi.Data.Models;
using System.Collections.Generic;
using TourismApi.Helper;

namespace TourismApi.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client>  Clients{ get; set; }
        public DbSet<Admin>  Admins{ get; set; }

        public DbSet<ServiceOwner> ServiceOwners { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<CarRental> Cars { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Like> Likes { get; set; }

        public DbSet<Booking> Booking { get; set; }

    }
}
