using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MauiCrudApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mauicrud.db3");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial data
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "First Item", Description = "This is the first item", CreatedAt = DateTime.Now, IsCompleted = false },
                new Item { Id = 2, Name = "Second Item", Description = "This is the second item", CreatedAt = DateTime.Now, IsCompleted = true },
                new Item { Id = 3, Name = "Third Item", Description = "This is the third item", CreatedAt = DateTime.Now, IsCompleted = false }
            );
        }
    }
}
