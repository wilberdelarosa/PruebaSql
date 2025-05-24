using System.Windows.Input;
using MauiCrudApp.Data;

namespace MauiCrudApp.ViewModels
{
    [QueryProperty(nameof(ItemId), "id")]
    public class ItemDetailViewModel : BaseViewModel
    {
        private readonly AppDbContext _dbContext;
        
        private int? itemId;
        public int? ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemAsync(value);
            }
        }

        private Item item;
        public Item Item
        {
            get => item;
            set
            {
                item = value;
                OnPropertyChanged();
            }
        }

        public bool IsNewItem => ItemId == null;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        public ItemDetailViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
            SaveCommand = new Command(async () => await SaveItemAsync());
            DeleteCommand = new Command(async () => await DeleteItemAsync(), () => !IsNewItem);
            CancelCommand = new Command(async () => await GoBackAsync());
            
            // Initialize with a new item
            Item = new Item();
        }

        private async void LoadItemAsync(int? itemId)
        {
            if (itemId == null)
            {
                Title = "New Item";
                Item = new Item();
                return;
            }

            Title = "Edit Item";
            
            try
            {
                IsBusy = true;
                Item = await _dbContext.Items.FindAsync(itemId.Value);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to load item: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveItemAsync()
        {
            if (string.IsNullOrWhiteSpace(Item.Name))
            {
                await Shell.Current.DisplayAlert("Error", "Name is required", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                if (IsNewItem)
                {
                    Item.CreatedAt = DateTime.Now;
                    _dbContext.Items.Add(Item);
                }
                else
                {
                    _dbContext.Items.Update(Item);
                }

                await _dbContext.SaveChangesAsync();
                await GoBackAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to save item: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteItemAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Item", 
                "Are you sure you want to delete this item?", 
                "Yes", "No");

            if (!confirm)
                return;

            try
            {
                IsBusy = true;
                _dbContext.Items.Remove(Item);
                await _dbContext.SaveChangesAsync();
                await GoBackAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to delete item: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
