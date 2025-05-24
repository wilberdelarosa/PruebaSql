using MauiCrudApp.ViewModels;

namespace MauiCrudApp.Views;

public partial class ItemsPage : ContentPage
{
    private readonly ItemsViewModel _viewModel;
    
    public ItemsPage(ItemsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}
