using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { DisplayOrder = 5, Id = 1, Name = "Fiction"},
                new Category { DisplayOrder = 8, Id = 2, Name = "Action" },
                new Category { DisplayOrder = 1, Id = 3, Name = "History" }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Blood Money", Author = "Peter Schweizer", ISBN = "ISBN-13", ListPrice = 20, Price = 20, Price50 = 16.99, Price100 = 15, Description= "China is killing Americans and working aggres­sively to maximize the carnage while our leaders remain passive and, in some cases, compliant. Why?", CategoryId=1, ImageUrl = "" },
                new Product { Id = 2, Name = "The Women: A Novel", Author = " Kristin Hannah", ISBN = "ISBN-13", ListPrice = 20, Price = 20, Price50 = 16.99, Price100 = 15, Description = "", CategoryId = 2, ImageUrl = "" },
                new Product { Id = 3, Name = "Worthy: How to Believe You Are Enough and Transform", Author = "Jamie Kern Lima", ISBN = "ISBN-13", ListPrice = 20, Price = 20, Price50 = 16.99, Price100 = 15, Description = "Imagine what you'd do, if you FULLY believed in YOU! When you stop doubting your greatness, build unshakable self-worth and embrace who you are, you transform your entire life! WORTHY teaches you how, with simple steps that lead to life-changing results!", CategoryId = 3, ImageUrl="" }
                );
        }
    }
}
