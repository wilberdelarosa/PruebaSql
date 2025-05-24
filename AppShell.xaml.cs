using MauiCrudApp.Views;

namespace MauiCrudApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
    }
}
