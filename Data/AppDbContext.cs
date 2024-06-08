using Microsoft.EntityFrameworkCore;
using MvcCrudApp.Models;

namespace MvcCrudApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure all string properties are varchar(255) by default
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetColumnType() == null)
                    {
                        property.SetColumnType("varchar(255)");
                    }
                }
            }

            // Configure Product Id as a string
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasMaxLength(10); // Adjust the length as needed
        }
    }
}
