using PropertyChanged;
using System.Threading.Tasks;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{   
    [AddINotifyPropertyChangedInterface]
    public class MenuPageViewModel
    {
        public ICommand MoveToCatalogPageCommand => new Command(MoveToCatalogPage);
        public INavigation Navigation { get; set; }
        public States StatesPage { get; set; } = States.Loading;
        public bool IsEnabled { get; set; } = true;

        public MenuPageViewModel()
        {
            StatesPage = States.Normal;
        }        

        /// <summary>
        /// Открывает страницу со справочниками
        /// </summary>
        private async void MoveToCatalogPage()
        {
            IsEnabled = false;
            StatesPage = States.Loading;

            await Navigation.PushAsync(await Task.Run(()=> new ManualPage()));

            StatesPage = States.Normal;
            IsEnabled = true;           
        }

    }
}
