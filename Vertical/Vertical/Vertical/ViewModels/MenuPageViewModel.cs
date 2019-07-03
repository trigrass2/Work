using PropertyChanged;
using System.Windows.Input;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{   
    [AddINotifyPropertyChangedInterface]
    public class MenuPageViewModel
    {        
        public ICommand MoveToChekPageCommand => new Command(MoveToChekPage);
        public ICommand MoveToCatalogPageCommand => new Command(MoveToCatalogPage);
        public ICommand MoveToRequestPageCommand => new Command(MoveToRequestPage);
        public ICommand UpdateContentCommand => new Command(UpdateContent);

        public INavigation Navigation { get; set; }

        public States StatesPage { get; set; } = States.Loading;
        public bool IsEnabled { get; set; } = true;

        public MenuPageViewModel()
        {
            StatesPage = States.Normal;
        }        

        /// <summary>
        /// Открывает страницу с чеклистами
        /// </summary>
        private async void MoveToChekPage()
        {
           
        }

        /// <summary>
        /// Открывает страницу со справочниками
        /// </summary>
        private async void MoveToCatalogPage()
        {
            IsEnabled = false;
            await Navigation.PushAsync(new ManualPage());
            IsEnabled = true;
           
        }

        /// <summary>
        /// Открывает страницу с заявками
        /// </summary>
        private async void MoveToRequestPage()
        {
            StatesPage = States.NoInternet;
        }
        
        /// <summary>
        /// Обновление страницы
        /// </summary>
        private void UpdateContent()
        {

        }
    }
}
