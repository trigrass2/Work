using ServiceDesk.Views;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        private ICommand _okCommand;
        public ICommand OkCommand
            => _okCommand ?? (_okCommand = new Command<string>(GoToAuth));

        public async void GoToAuth(string provider)
        {
            switch (provider)
            {
                case "google": await Navigation.PushAsync(new GoogleAuthView()); break;
                case "1c": await Navigation.PushAsync(new AuthView()); break;
                case "Local": await Navigation.PushAsync(new LocalAuthPage()); break;
            }
        }
    }
}
