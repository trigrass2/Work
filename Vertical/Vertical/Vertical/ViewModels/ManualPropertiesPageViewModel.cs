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
        public States States { get; set; } = States.Loading;

        public ICommand GoToCreatePropertyPageCommand => new Command(GoToCreatePropertyPage);
        public ICommand GoToEditPropertyPageCommand => new Command(GoToEditPropertyPage);
        public ICommand RefreshCommand => new Command(UpdateSystemPropertyModels);

        public ObservableCollection<SystemPropertyModel> SystemPropertyModels { get; set; }
        public SystemPropertyModel SelectedPropertyModel { get; set; }

        public bool IsEnabled { get; set; } = true;

        public ManualPropertiesPageViewModel()
        {
            SystemPropertyModels = new ObservableCollection<SystemPropertyModel>();
            UpdateSystemPropertyModels();
            States = States.Normal;
        }

        public async void UpdateSystemPropertyModels()
        {
            if (!NetworkCheck.IsInternet())
            {
                States = States.NoInternet;
                return;
            }

            SystemPropertyModels.Clear();
            var items = await Api.GetDataFromServerAsync<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { ShowHidden  = true});
            try
            {
                foreach (var s in items)
                {
                    SystemPropertyModels.Add(s);
                }
            }catch(Exception ex)
            {
                
                Log.WriteLine(LogPriority.Error, $"{nameof(UpdateSystemPropertyModels)}", $"{ex.Message}");
            }
            States = SystemPropertyModels.Count > 0 ? States.Normal : States.NoData;
        }


        private async void GoToCreatePropertyPage()
        {
            IsEnabled = false;
            await Navigation.PushModalAsync(new CreatePropertyPage("AddSystemProperty", new SystemPropertyModel()));
            IsEnabled = true;
        }

        private async void GoToEditPropertyPage(object commandParameter)
        {
            IsEnabled = false;
            await Navigation.PushModalAsync(new CreatePropertyPage("EditSystemProperty", commandParameter as SystemPropertyModel));
            IsEnabled = true;
        }
    }
}
