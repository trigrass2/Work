using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Vertical.CustomViews;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{    
    public class CheckPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;
        public ICommand CreatePropertiesValuesCommand => new Command(CreatePropertiesValues);        

        public SystemObjectModel SystemObjectModel { get; set; }
        public ObservableCollection<GroupingModel<SystemObjectPropertyValueModel>> SystemPropertyModels { get; set; }
        public InputAddSystemObjectPropertiesValues Property { get; set; }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemObjectModel = obj;
            Property = new InputAddSystemObjectPropertiesValues();
            SystemPropertyModels = new ObservableCollection<GroupingModel<SystemObjectPropertyValueModel>>();
            UpdateSystemPropertyModels();
            Property = new InputAddSystemObjectPropertiesValues { ObjectGUID = SystemObjectModel?.GUID };
        }

        private void UpdateSystemPropertyModels()
        {
            SystemPropertyModels.Clear();

            var values = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            //var properties = Api.GetDataFromServer<SystemObjectTypePropertyModel>("SystemManagement/GetSystemObjectTypeProperties", new { ObjectTypeID = SystemObjectModel.TypeID });            
            var groups = values?.OrderBy(o => o.GroupID).Select(x => x.GroupName).Distinct();

            foreach (var s in groups?.AsParallel().Select(x => GroupingModel<SystemObjectPropertyValueModel>.GetGroup(x, values)))
            {
                SystemPropertyModels.Add(s);
            }           
        }

        private void CreatePropertiesValues(object obj)
        {
            throw new NotImplementedException();
        }
        //private GroupingModel<SystemObjectPropertyValueModel> GetGroup(string nameGroup, IList<SystemObjectPropertyValueModel> items)
        //{            
        //    GroupingModel<SystemObjectPropertyValueModel> groupProperties = new GroupingModel<SystemObjectPropertyValueModel>(nameGroup);

        //    foreach (var i in items.Where(x => x.GroupName == nameGroup))
        //    {
        //        groupProperties.Add(i);
        //    }
        //    return groupProperties;
        //}

    }
}
