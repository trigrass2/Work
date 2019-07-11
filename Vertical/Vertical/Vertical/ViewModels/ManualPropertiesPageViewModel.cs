using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using static Vertical.Constants;
using Vertical.Models;
using Vertical.Services;

namespace Vertical.ViewModels
{    
    public class ManualPropertiesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;

        public ObservableCollection<SystemPropertyModel> SystemPropertyModels { get; set; }

        public ManualPropertiesPageViewModel()
        {
            SystemPropertyModels = new ObservableCollection<SystemPropertyModel>();
            UpdateSystemPropertyModels();
        }

        private void UpdateSystemPropertyModels()
        {
            SystemPropertyModels.Clear();

            foreach (var s in Api.GetDataFromServer<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { }))
            {
                SystemPropertyModels.Add(s);
            }
        }
    }
}
