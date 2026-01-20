using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<CartProducts> CartProducts { get; set; }
        public DbSet<DeliveryOption> DeliveryOption { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=WebShop;Trusted_Connection=True;TrustServerCertificate=True;");
            
        }
    }
}
