using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using chuloon.Models;

namespace chuloon.Data
{
    public class ChuloonContext : IdentityDbContext<ApplicationUser>
    {

        public ChuloonContext(DbContextOptions<ChuloonContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>().HasKey(o => o.Id);
            builder.Entity<Order>().HasOne<Pickup>(o => o.PickUp).WithMany(p => p.Orders).HasForeignKey(o => o.PickupId);
            builder.Entity<Order>().HasOne<ApplicationUser>(o => o.User);
            builder.Entity<Order>()
                .HasMany<OrderItem>()
                .WithOne();

            builder.Entity<OrderItem>().HasKey(o => o.Id);
            builder.Entity<OrderItem>().HasOne<MenuItem>(o => o.MenuItem);
            builder.Entity<OrderItem>().HasOne<Order>(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Restaurant> Restaurants { get; set; }
            public DbSet<MenuCategory> MenuCategories { get; set; }
            public DbSet<MenuItem> MenuItems { get; set; }
            public DbSet<MenuPrice> MenuPrices { get; set; }
            public DbSet<Pickup> Pickup { get; set; }
            public DbSet<Order> Order { get; set; }
            public DbSet<OrderItem> OrderItem { get; set; }

    }
}
