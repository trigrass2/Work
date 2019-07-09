using Vertical.Models;
using PropertyChanged;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace Vertical.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class InfoPageViewModel
    {
        public SystemObjectModel SystemObjectModel { get; set; }
        public ICommand BackCommand => new Command(Back);
        public INavigation Navigation { get; set; }

        private async void Back()
        {
            await Navigation.PopModalAsync();
        }

        public InfoPageViewModel(SystemObjectModel obj)
        {
            SystemObjectModel = obj;
        }
    }
}
