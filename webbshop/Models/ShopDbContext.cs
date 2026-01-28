using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Database;

namespace webbshop.Models
{
    public class ShopDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<DeliveryOption> DeliveryOptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer(DatabaseStrings.SqlConnectionString);
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartProduct>()
                .ToTable("CartProducts");
            // Ser till att sätta så vi inte kan ta bort saker som har kopplingar
            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.CartProducts)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

       
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.Product)
                .WithMany(p => p.PaymentHistories)
                .HasForeignKey(ph => ph.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.User)
                .WithMany(u => u.PaymentHistories)
                .HasForeignKey(ph => ph.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.PaymentOption)
                .WithMany(po => po.PaymentHistories)
                .HasForeignKey(ph => ph.PaymentOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.DeliveryOption)
                .WithMany(d => d.PaymentHistories)
                .HasForeignKey(ph => ph.DeliveryOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.DeliveryCity)
                .WithMany(c => c.PaymentHistories)
                .HasForeignKey(ph => ph.DeliveryCityId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(co => co.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.City)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.SetNull); 
        }
    }
}
