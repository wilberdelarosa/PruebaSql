using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MauiCrudApp.Data;
using MauiCrudApp.ViewModels;
using MauiCrudApp.Views;
using System.IO;

namespace MauiCrudApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Database context
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mauicrud.db3");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Filename={dbPath}"));

        // Initialize database
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }

        // Register ViewModels
        builder.Services.AddTransient<ItemsViewModel>();
        builder.Services.AddTransient<ItemDetailViewModel>();

        // Register Views
        builder.Services.AddTransient<ItemsPage>();
        builder.Services.AddTransient<ItemDetailPage>();

        // Register AppShell
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
