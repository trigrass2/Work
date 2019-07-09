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
            StatesPage = States.Loading;

            await Navigation.PushAsync(await Task.Run(()=> new ManualPage()));

            StatesPage = States.Normal;
            IsEnabled = true;
           
        }

        /// <summary>
        /// Открывает страницу с заявками
        /// </summary>
        private async void MoveToRequestPage()
        {
            
        }
        
        /// <summary>
        /// Обновление страницы
        /// </summary>
        private void UpdateContent()
        {

        }
    }
}
