using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using static Vertical.Constants;
using Vertical.Models;
using Vertical.Services;
using System.Windows.Input;
using Vertical.Views;
using System;
using Android.Util;

namespace Vertical.ViewModels
{    
    public class ManualPropertiesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;

        public ICommand GoToCreatePropertyPageCommand => new Command(GoToCreatePropertyPage);
        public ICommand GoToEditPropertyPageCommand => new Command(GoToEditPropertyPage);

        public ObservableCollection<SystemPropertyModel> SystemPropertyModels { get; set; }
        public SystemPropertyModel SelectedPropertyModel { get; set; }

        public ManualPropertiesPageViewModel()
        {
            SystemPropertyModels = new ObservableCollection<SystemPropertyModel>();
            UpdateSystemPropertyModels();
        }

        public void UpdateSystemPropertyModels()
        {
            SystemPropertyModels.Clear();
            var items = Api.GetDataFromServer<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { });
            try
            {
                foreach (var s in items)
                {
                    SystemPropertyModels.Add(s);
                }
            }catch(Exception ex)
            {
                Api.SendError($"{ex.Message}");
                Log.WriteLine(LogPriority.Error, $"{nameof(UpdateSystemPropertyModels)}", $"{ex.Message}");
            }
            States = States.Normal;
        }

        private async void GoToCreatePropertyPage()
        {
            await Navigation.PushModalAsync(new CreatePropertyPage("AddSystemProperty", new SystemPropertyModel()));
        }

        private async void GoToEditPropertyPage(object commandParameter)
        {
            await Navigation.PushModalAsync(new CreatePropertyPage("EditSystemProperty", commandParameter as SystemPropertyModel));
        }
    }
}
