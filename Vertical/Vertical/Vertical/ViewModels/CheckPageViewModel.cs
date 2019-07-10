using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;

namespace Vertical.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CheckPageViewModel
    {

        public INavigation Navigation { get; set; }
        public ICommand CreatePropertiesValuesCommand => new Command(UpdateSystemPropertyModels);
        public SystemObjectModel SystemObjectModel { get; set; }
        public ObservableCollection<SystemPropertyModel> SystemPropertyModels { get; set; }
        public InputAddSystemObjectPropertiesValues Property { get; set; }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemObjectModel = obj;
            
            SystemPropertyModels = new ObservableCollection<SystemPropertyModel>();
            UpdateSystemPropertyModels();
        }

        private void UpdateSystemPropertyModels()
        {
            SystemPropertyModels.Clear();

            var values = Api.GetDataFromServer<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { ObjectTypeID = SystemObjectModel.TypeID });
            var propert = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            Property = new InputAddSystemObjectPropertiesValues { ObjectGUID = SystemObjectModel?.GUID, PropertyID = values[0].ID };
            foreach (var v in values)
            {
                SystemPropertyModels.Add(v);
            }            
        }        
    }
}
