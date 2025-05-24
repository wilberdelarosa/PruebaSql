using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiCrudApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy;
        public bool IsBusy 
        { 
            get => isBusy; 
            set 
            { 
                isBusy = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(IsNotBusy));
            } 
        }
        
        public bool IsNotBusy => !IsBusy;

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnAppearing() { }
    }
}
