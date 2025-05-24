using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace MauiCrudApp.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "mauicrud.db3");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
            
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
