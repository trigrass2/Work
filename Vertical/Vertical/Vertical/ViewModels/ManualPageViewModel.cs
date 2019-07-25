using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Vertical.Views;
using static Vertical.Constants;
using System.Threading.Tasks;

namespace Vertical.ViewModels
{
    public class ManualPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;

        public bool IsEnabled { get; set; } = true;

        public ICommand GoToManualObjectsPageCommand => new Command(GoToManualObjectsPage);
        public ICommand GoToManualPropertiesPageCommand => new Command(GoToManualPropertiesPage);
        public ICommand GoToManualTypesObjectsPageCommand => new Command(GoToManualTypesObjectsPage);

        public ManualPageViewModel()
        {

        }

        private async void GoToManualObjectsPage()
        {
            States = States.Loading;
            IsEnabled = false;
            
            await Navigation.PushAsync(await Task.Run(() => new ManualObjectsPage()));

            IsEnabled = true;
            States = States.Normal;
        }

        private async void GoToManualPropertiesPage()
        {
            States = States.Loading;
            IsEnabled = false;
            
            await Navigation.PushAsync(await Task.Run(() => new ManualTypesObjectsPage()));

            IsEnabled = true;
            States = States.Normal;
        }

        private async void GoToManualTypesObjectsPage()
        {
            States = States.Loading;
            IsEnabled = false;            

            await Navigation.PushAsync(await Task.Run(() => new ManualPropertiesPage()));

            IsEnabled = true;
            States = States.Normal;
        }
    }
}
