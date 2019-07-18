using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Vertical.CustomViews;
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
        public ObservableCollection<Grouping<SystemObjectTypePropertyModel>> SystemPropertyModels { get; set; }
        public InputAddSystemObjectPropertiesValues Property { get; set; }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemObjectModel = obj;
            Property = new InputAddSystemObjectPropertiesValues();
            SystemPropertyModels = new ObservableCollection<Grouping<SystemObjectTypePropertyModel>>();
            UpdateSystemPropertyModels();
            Property = new InputAddSystemObjectPropertiesValues { ObjectGUID = SystemObjectModel?.GUID };
        }

        private void UpdateSystemPropertyModels()
        {
            SystemPropertyModels.Clear();

            var properties = Api.GetDataFromServer<SystemObjectTypePropertyModel>("SystemManagement/GetSystemObjectTypeProperties", new { ObjectTypeID = SystemObjectModel.TypeID });
            //var values = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            
            var groups = properties.GroupBy(p => p.GroupName).Select(g => new Grouping<SystemObjectTypePropertyModel>(g.Key, g));

            foreach (var v in groups)
            {
                SystemPropertyModels.Add(v);
            }            
        }      
        
        private void CreatePropertiesValues()
        {

        }
    }
}
