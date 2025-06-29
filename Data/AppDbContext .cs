using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Office_Name);
                entity.HasIndex(e => e.Office_Name).IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Product_Name);
                entity.HasIndex(p => p.Product_Name).IsUnique();
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasIndex(p => new
                {
                    p.Purchase_or_Sale,
                    p.Purchased_Date,
                    p.Product_Name,
                    p.Seller_Name
                }).IsUnique();
            });
        }
    }
}
