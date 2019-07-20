using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
        public States States { get; set; } = States.Loading;
        public ICommand SavePropertiesValuesCommand => new Command(SavePropertiesValuesAsync);
        public ICommand AddNewObjectInPropertyCommand => new Command(AddNewObjectInPropperty);
        public bool IsEnabled { get; set; } = true;
        public bool IsVisibleSaveButton { get; set; }
         
        public SystemObjectModel SystemObjectModel { get; set; }
        public ObservableCollection<GroupingModel<SystemObjectPropertyValueModel>> SystemPropertyModels { get; set; }
        public ObservableCollection<AddSystemObjectPropertyValueModel> NewValues { get; set; }

        public CheckPageViewModel(SystemObjectModel obj)
        {            
            SystemObjectModel = obj;            
            NewValues = new ObservableCollection<AddSystemObjectPropertyValueModel>();
            SystemPropertyModels = new ObservableCollection<GroupingModel<SystemObjectPropertyValueModel>>();
            UpdateSystemPropertyModels();           
            
        }

        private async void AddNewObjectInPropperty(object commandParameter)
        {
            var prop = commandParameter as SystemObjectPropertyValueModel;
            //var neqw = SystemPropertyModels.Select(x => x.Where(k => k.GroupID == prop.GroupID));
            if (prop.SourceObjectGUID != null)
            {
                var objects = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = prop.SourceObjectGUID });
            }
            var types = Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes");
            var action = await Application.Current.MainPage
                                                  .DisplayActionSheet(
                                                  "Тип нового объекта",
                                                  "отмена",
                                                  null,
                                                  types.Select(x => x.Name).ToArray());
            if(action != null && action != "отмена")
            {
                States = States.Loading;
                int typeId = types.Where(x => x.Name == action).Select(x => x.ID).FirstOrDefault();
                Api.SendDataToServer("System/AddSystemObject", new { Name = "Коля", TypeID = typeId, ParentGUID = prop.SystemObjectGUID });
                UpdateSystemPropertyModels();
            }
            
        }

        private void UpdateSystemPropertyModels()
        {
            
            SystemPropertyModels.Clear();

            var values = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            if(values.Count == 0)
            {
                States = States.NoData;
                return;
            }
                      
            var groups = values?.OrderBy(o => o.GroupID).Select(x => x.GroupName).Distinct();
            
            //foreach (var s in groups?.AsParallel().Select(x => GetGroup(x, values)))
            //{
            //    SystemPropertyModels.Add(s);
            //}

            foreach (var s in groups)
            {
                SystemPropertyModels.Add(GetGroup(s, values));
            }

            States = States.Normal;
        }

        private async void SavePropertiesValuesAsync(object obj)
        {
            IsEnabled = false;
            await Task.Run(() => {
                
                for (int i = NewValues.Count-1; i > 0; i--)
                {
                    if (NewValues[i].Value == null) NewValues.Remove(NewValues[i]);
                }
                
                foreach (var n in NewValues)
                {
                    Api.SendDataToServer("System/AddSystemObjectPropertyValue", n);
                }
                NewValues.Clear();
            });
            
            IsVisibleSaveButton = false;
        }

        public void CreateNewValue(SystemObjectPropertyValueModel property, object value)
        {            
            var item = new AddSystemObjectPropertyValueModel
            {
                ObjectGUID = SystemObjectModel?.GUID,
                PropertyID = property?.ID,
                PropertyNum = property?.Num,
                Value = value,
                ValueNum = property.ValueNum
            };
            
            if (NewValues.Any(x => x.PropertyID == item.PropertyID && x.Value != item.Value))
            {
                NewValues[NewValues.IndexOf(NewValues.Where(x => x.PropertyID == item.PropertyID).FirstOrDefault())] = item;
                
                IsVisibleSaveButton = true;
            }
            else if(value != null)
            {
                NewValues.Add(item);
                IsVisibleSaveButton = true;
            }

        }

        private GroupingModel<SystemObjectPropertyValueModel> GetGroup(string nameGroup, IList<SystemObjectPropertyValueModel> items)
        {
            GroupingModel<SystemObjectPropertyValueModel> groupProperties = new GroupingModel<SystemObjectPropertyValueModel>(nameGroup);

            foreach (var i in items.Where(x => x.GroupName == nameGroup))
            {
                groupProperties.Add(i);
            }
            return groupProperties;
        }
    }
}
