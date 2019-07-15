using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;

namespace Vertical.ViewModels
{    
    public class CheckPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        //public ICommand CreatePropertiesValuesCommand => new Command(CreatePropertiesValues);
        public SystemObjectModel SystemObjectModel { get; set; }
        public ObservableCollection<SystemPropertyModel> SystemPropertyModels { get; set; }
        public InputAddSystemObjectPropertiesValues Property { get; set; }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemObjectModel = obj;
            Property = new InputAddSystemObjectPropertiesValues();
            SystemPropertyModels = new ObservableCollection<SystemPropertyModel>();
            UpdateSystemPropertyModels();
            Property = new InputAddSystemObjectPropertiesValues { ObjectGUID = SystemObjectModel?.GUID };
        }

        private void UpdateSystemPropertyModels()
        {
            SystemPropertyModels.Clear();

            var values = Api.GetDataFromServer<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { ObjectTypeID = SystemObjectModel.TypeID });
            var propert = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            
            foreach (var v in values)
            {
                SystemPropertyModels.Add(v);
            }            
        }      
        
        private void CreatePropertiesValues()
        {

        }
    }
}
