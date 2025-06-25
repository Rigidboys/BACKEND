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
        public DbSet<Purchase> Purchases { get; set; } // ✅ 복수형으로 수정
        public DbSet<User> Users { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasIndex(c => c.Office_Name).IsUnique();

            modelBuilder.Entity<Product>().HasIndex(p => p.Product_Name).IsUnique();

            modelBuilder
                .Entity<Purchase>()
                .HasIndex(p => new
                {
                    p.Customer_Name,
                    p.Purchased_Date,
                    p.Product_Name,
                })
                .IsUnique();
        }
    }
}
