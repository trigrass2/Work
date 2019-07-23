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
using Acr.UserDialogs;
using System;

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
        public ObservableCollection<AddSystemObjectPropertyValueModel> StartValues { get; set; }

        public CheckPageViewModel() { }

        public CheckPageViewModel(SystemObjectModel obj)
        {            
            SystemObjectModel = obj;            
            NewValues = new ObservableCollection<AddSystemObjectPropertyValueModel>();
            StartValues = new ObservableCollection<AddSystemObjectPropertyValueModel>();
            SystemPropertyModels = new ObservableCollection<GroupingModel<SystemObjectPropertyValueModel>>();
            UpdateSystemPropertyModels();           
            
        }

        /// <summary>
        /// добавляет новый объект в качестве свойства
        /// </summary>
        /// <param name="commandParameter"></param>
        private async void AddNewObjectInPropperty(object commandParameter)
        {
            var prop = commandParameter as SystemObjectPropertyValueModel;
            
            if (string.IsNullOrEmpty(prop.SourceObjectGUID)){

                var types = Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes");
                var action = await Application.Current.MainPage
                                                  .DisplayActionSheet(
                                                  "Тип нового объекта",
                                                  "отмена",
                                                  null,
                                                  types.Select(x => x.Name).ToArray());
                if (!string.IsNullOrEmpty(action) && action != "отмена")
                {                    
                    int typeId = types.Where(x => x.Name == action).Select(x => x.ID).FirstOrDefault();
                    PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                    {
                        InputType = InputType.Name,
                        OkText = "Создать",
                        Title = "Создание объекта"                        
                    });

                    using (UserDialogs.Instance.Loading("Создание объекта...", null, null, true, MaskType.Black))
                    {
                        if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
                        {
                            string guidNewItem = Api.AddSystemObject(new { Name = pResult.Text, TypeID = typeId, ParentGUID = prop.SystemObjectGUID });
                            if(guidNewItem != default(string))
                            {
                                prop.Value = guidNewItem;
                                await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                                    new
                                    {
                                        ObjectGUID = SystemObjectModel?.GUID,
                                        PropertyID = prop.ID,
                                        PropertyNum = prop.Num,
                                        Value = prop.Value,
                                        ValueNum = prop.ValueNum
                                    });

                            }
                            UpdateSystemPropertyModels();
                        }
                    }                                         
                }
            }
            else
            {
                var objects = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = prop.SourceObjectGUID });
            }
            
        }

        /// <summary>
        /// Обновляет источник данных
        /// </summary>
        private void UpdateSystemPropertyModels()
        {            
            SystemPropertyModels.Clear();

            var values = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            if(values.Count == 0)
            {
                States = States.NoData;
                return;
            }
            var nullsBool = values.Where(x => x?.TypeID == 1);            

            if(nullsBool.Where(x => x?.Value == null).Count() > 0)
            {
                foreach (var n in nullsBool)
                {
                    Api.SendDataToServer("System/AddSystemObjectPropertyValue", new { ObjectGUID = SystemObjectModel?.GUID, PropertyID = n?.ID, PropertyNum = n?.Num, Value = false, n?.ValueNum });
                }
                values = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            }
            
            NewValues.Clear();
            StartValues.Clear();
            foreach (var n in nullsBool)
            {
                var item = new AddSystemObjectPropertyValueModel
                {
                    ObjectGUID = SystemObjectModel?.GUID,
                    PropertyID = n.ID,
                    PropertyNum = n.Num,
                    Value = n?.Value,
                    ValueNum = n.ValueNum
                };
                NewValues.Add(item);
                StartValues.Add(item);
            }
            
            var groups = values?.OrderBy(o => o.GroupID).Select(x => x.GroupName).Distinct();

            try
            {
                foreach (var s in groups)
                {
                    SystemPropertyModels.Add(GetGroup(s, values));
                }
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, "In foreach (var s in groups){} ->", ex.Message);
            }
            

            States = States.Normal;
        }

        /// <summary>
        /// Отправляет изменения на сервер
        /// </summary>
        private async void SavePropertiesValuesAsync()
        {
            IsEnabled = false;
            using (UserDialogs.Instance.Loading("Сохранение изменений...", null, null, true, MaskType.Black))
            {
                await Task.Run(() => {

                    var items = NewValues.Except(StartValues.Intersect(NewValues));

                    foreach (var n in items)
                    {
                        if(Api.SendDataToServer("System/AddSystemObjectPropertyValue", n) == false)
                        {
                            UserDialogs.Instance.Alert("Не удалось создать", null, "Ок");
                            return;
                        }
                        
                    }
                    Device.BeginInvokeOnMainThread(() => UpdateSystemPropertyModels());
                });
                
                IsVisibleSaveButton = false;
                IsEnabled = true;
            }
            
        }

        public void CreateNewValue(SystemObjectPropertyValueModel property, object value)
        {
            var objectForDelete = NewValues;
            var item = new AddSystemObjectPropertyValueModel
            {
                ObjectGUID = SystemObjectModel?.GUID,
                PropertyID = property?.ID,
                PropertyNum = property?.Num,
                Value = value,
                ValueNum = property.ValueNum
            };

            if(property.TypeID == 1)
            {
                if (NewValues.Any(x => x.PropertyID == item.PropertyID && !x.Value.Equals(item.Value)))
                {
                    NewValues[NewValues.IndexOf(NewValues.Where(x => x.PropertyID.Equals(item.PropertyID)).First())] = item;
                }
            }
            else
            {
                NewValues.Add(item);
            }

            IsVisibleSaveButton = StartValues.SequenceEqual(NewValues) == true ? false : true;
        }

        private GroupingModel<SystemObjectPropertyValueModel> GetGroup(string nameGroup, IList<SystemObjectPropertyValueModel> items)
        {
            try
            {
                GroupingModel<SystemObjectPropertyValueModel> groupProperties = new GroupingModel<SystemObjectPropertyValueModel>(nameGroup);

                foreach (var i in items.Where(x => x.GroupName == nameGroup))
                {
                    groupProperties.Add(i);
                }
                return groupProperties;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, "In GetGroup ->", ex.Message);
                return default(GroupingModel<SystemObjectPropertyValueModel>);
            }
            
        }
    }
}
