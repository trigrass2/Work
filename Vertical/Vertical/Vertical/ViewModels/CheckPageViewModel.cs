using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;
using static Vertical.Constants;
using Acr.UserDialogs;
using System;
using Syncfusion.DataSource;

namespace Vertical.ViewModels
{    
    public class CheckPageViewModel : BaseViewModel
    {
        public ICommand SavePropertiesValuesCommand => new Command(SavePropertiesValuesAsync);
        public ICommand AddNewObjectInPropertyCommand => new Command(AddNewObjectInPropperty);

        public bool IsVisibleSaveButton { get; set; }

        public SystemObjectModel SystemObjectModel { get; set; }
        public ObservableCollection<SystemObjectPropertyValueModel> SystemPropertyModels { get; set; }
        public ObservableCollection<AddSystemObjectPropertyValueModel> NewValues { get; set; }
        public ObservableCollection<AddSystemObjectPropertyValueModel> StartValues { get; set; }
        public ObservableCollection<SystemObjectModel> ObjectModels { get; set; }

        public DataSource SourceObjects { get; set; }

        public CheckPageViewModel() { }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemObjectModel = obj;
            SourceObjects = new DataSource();
            SourceObjects.GroupDescriptors.Add(new GroupDescriptor("GroupName"));
            SourceObjects.GroupDescriptors.Add(new GroupDescriptor("ID"));            
            NewValues = new ObservableCollection<AddSystemObjectPropertyValueModel>();
            StartValues = new ObservableCollection<AddSystemObjectPropertyValueModel>();
            SystemPropertyModels = new ObservableCollection<SystemObjectPropertyValueModel>();
            ObjectModels = new ObservableCollection<SystemObjectModel>();
            UpdateSystemPropertyModels();            
        }

        private async Task CreateArrangement(SystemObjectPropertyValueModel property)
        {
            var typeName = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = property.SystemObjectGUID})
                .Where(x => x.TypeID == property.SourceObjectTypeID).Select(n => n.TypeName).FirstOrDefault();
            
            string guidNewItem = await Api.AddSystemObjectAsync(new { Name = typeName, TypeID = property.SourceObjectTypeID, ParentGUID = property.SystemObjectGUID });
            if (guidNewItem != default(string))
            {
                int valueNum = 0;
                if (property.Value != null)
                {
                    var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                    valueNum = v.Max(x => x.ValueNum);
                }

                property.Value = guidNewItem;
                await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                    new
                    {
                        ObjectGUID = SystemObjectModel?.GUID,
                        PropertyID = property.ID,
                        PropertyNum = property.Num,
                        Value = property.Value,
                        ValueNum = valueNum + 1
                    });
                Device.BeginInvokeOnMainThread(UpdateSystemPropertyModels);
            }
        }

        /// <summary>
        /// добавляет новый объект в качестве свойства
        /// </summary>
        /// <param name="commandParameter"></param>
        private async void AddNewObjectInPropperty(object commandParameter)
        {
            
            var prop = commandParameter as SystemObjectPropertyValueModel;
            if(prop.SourceObjectTypeID != null)
            {
                using (UserDialogs.Instance.Loading("Создание...", null,null,true,MaskType.Black))
                {
                    await CreateArrangement(prop);
                }
                
            }
            else
            {
                if (string.IsNullOrEmpty(prop.SourceObjectParentGUID))
                {
                    var types = await Api.GetDataFromServerAsync<SystemObjectTypeModel>("System/GetSystemObjectTypes");
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
                                string guidNewItem = await Api.AddSystemObjectAsync(new { Name = pResult.Text, TypeID = typeId, ParentGUID = prop.SystemObjectGUID });
                                if (guidNewItem != default(string))
                                {
                                    int valueNum = 0;
                                    if (prop.Value != null)
                                    {
                                        var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                                        valueNum = v.Max(x => x.ValueNum);
                                    }

                                    prop.Value = guidNewItem;
                                    await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                                        new
                                        {
                                            ObjectGUID = SystemObjectModel?.GUID,
                                            PropertyID = prop.ID,
                                            PropertyNum = prop.Num,
                                            Value = prop.Value,
                                            ValueNum = valueNum + 1
                                        });

                                    await Device.InvokeOnMainThreadAsync(() => UpdateSystemPropertyModels());
                                }

                            }
                        }

                    }
                }
                else
                {
                    var objects = await Api.GetDataFromServerAsync<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = prop.SourceObjectParentGUID });
                    var action = await Application.Current.MainPage
                                                      .DisplayActionSheet(
                                                      "",
                                                      "отмена",
                                                      null,
                                                      objects.Select(x => x.Name).ToArray());
                    if (!string.IsNullOrEmpty(action) && action != "отмена")
                    {
                        using (UserDialogs.Instance.Loading("Создание объекта...", null, null, true, MaskType.Black))
                        {
                            var item = objects.Where(x => x.Name == action).FirstOrDefault();
                            int valueNum = 0;
                            if (prop.Value != null)
                            {
                                var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                                valueNum = v.Max(x => x.ValueNum);
                            }
                            prop.Value = item.GUID;
                            await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                                         new
                                         {
                                             ObjectGUID = SystemObjectModel?.GUID,
                                             PropertyID = prop.ID,
                                             PropertyNum = prop.Num,
                                             Value = prop.Value,
                                             ValueNum = valueNum + 1
                                         });

                            await Device.InvokeOnMainThreadAsync(() => UpdateSystemPropertyModels());

                        }
                    }
                }
            }
            

            //if(prop.SourceObjectTypeID != null)
            //{
            //    PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            //    {
            //        InputType = InputType.Name,
            //        OkText = "Создать",
            //        Title = "Новый объект"
            //    });
            //    if (pResult.Ok)
            //    {
            //        using (UserDialogs.Instance.Loading("Создание объекта...", null, null, true, MaskType.Black))
            //        {
            //            await CreateArrangement(pResult.Text, prop).ContinueWith((task) => UserDialogs.Instance.HideLoading());
            //        }
            //    }


            //}
            //else
            //{

            //}


        }

        private async Task UpdateSystemPropertyModelsAsync()
        {
            await Task.Run(()=> UpdateSystemPropertyModels());
        }
        /// <summary>
        /// Обновляет источник данных
        /// </summary>
        private void UpdateSystemPropertyModels()
        {            
            SystemPropertyModels.Clear();

            var values = Api.GetDataFromServer<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            
            if (values.Count == 0)
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
            
            try
            {
                foreach (var s in values?.OrderBy(o => o.GroupID))
                {
                    SystemPropertyModels.Add(s);                    
                }
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, "In foreach (var s in groups){} ->", ex.Message);
            }
            
            SourceObjects.Source = SystemPropertyModels;
            
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
                var items = NewValues.Except(StartValues.Intersect(NewValues));

                foreach (var n in items)
                {
                    if (await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue", n) == false)
                    {
                        UserDialogs.Instance.Alert("Не удалось создать", null, "Ок");
                        return;
                    }

                }

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
            NewValues.Where(x => x?.Value is null).AsParallel().ForAll(x => x.Value = false);
            if(property.TypeID == 1)
            {
                if (NewValues.Any(x => x.PropertyID == item.PropertyID && x?.Value is null))
                {
                    NewValues[NewValues.IndexOf(NewValues.Where(x => x.PropertyID == item.PropertyID).First())] = item;
                }
                else if(NewValues.Any(x => x.PropertyID == item.PropertyID &&  !x.Value.Equals(item.Value)))
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
    }
}
