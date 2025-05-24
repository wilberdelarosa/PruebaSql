using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiCrudApp.Data;
using MauiCrudApp.Views;
using Microsoft.EntityFrameworkCore;

namespace MauiCrudApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly AppDbContext _dbContext;
        
        public ObservableCollection<Item> Items { get; } = new();
        
        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand ItemTappedCommand { get; }
        public ICommand RefreshCommand { get; }

        public ItemsViewModel(AppDbContext dbContext)
        {
            Title = "Items";
            _dbContext = dbContext;
            
            LoadItemsCommand = new Command(async () => await LoadItemsAsync());
            AddItemCommand = new Command(async () => await GoToItemDetailAsync());
            ItemTappedCommand = new Command<Item>(async (item) => await GoToItemDetailAsync(item));
            RefreshCommand = new Command(async () => await RefreshItemsAsync());
        }

        public override async void OnAppearing()
        {
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Items.Clear();
                
                var items = await _dbContext.Items.AsNoTracking().ToListAsync();
                
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to load items: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshItemsAsync()
        {
            await LoadItemsAsync();
        }

        private async Task GoToItemDetailAsync(Item item = null)
        {
            if (item == null)
            {
                // Navigate to create a new item
                await Shell.Current.GoToAsync(nameof(ItemDetailPage));
            }
            else
            {
                // Navigate to edit an existing item
                var route = $"{nameof(ItemDetailPage)}?id={item.Id}";
                await Shell.Current.GoToAsync(route);
            }
        }
    }
}
