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
using System.Collections.Generic;
using System.Net;

namespace Vertical.ViewModels
{    
    public class CheckPageViewModel : BaseViewModel
    {
        //public ICommand SavePropertiesValuesCommand => new Command(SavePropertiesValuesAsync);
        public ICommand AddNewObjectInPropertyCommand => new Command(AddNewObjectInPropperty);
        public ICommand EditObjectCommand => new Command(EditObject);
        public ICommand IsCheckedCommand => new Command(IsChecked);
        public ICommand DeletePropertyCommand => new Command(DeleteObjectProperty);
        //public ICommand ChangeTextCommand => new Command(CreateNewValue);

        public bool IsVisibleSaveButton { get; set; }
        public bool IsVisibleButtons { get; set; }

        public SystemObjectModel SystemObjectModel { get; set; }
        //public ObservableCollection<SystemObjectPropertyValueModel> SystemPropertyModels { get; set; }
        public List<AddSystemObjectPropertyValueModel> NewValues { get; set; }
        public NotifyTaskCompletion<DataSource> Source { get; set; }
        public NotifyTaskCompletion<DataSource> SourceObj { get; set; }
        public DataSource SourceObjects { get; set; }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }

        private int? PropertyID { get; set; }

        public CheckPageViewModel() { }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            MinDate = DateTime.Now.AddYears(-1);
            MaxDate = DateTime.Now.AddYears(1);
            SystemObjectModel = obj;
                    
            NewValues = new List<AddSystemObjectPropertyValueModel>();
            //SystemPropertyModels = new ObservableCollection<SystemObjectPropertyValueModel>();
            Source = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels());
            //SourceObj = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels(2));
        }
        

        private async void IsChecked(object obj)
        {
            var model = obj as SystemObjectPropertyValueModel;
            await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                                            new
                                            {
                                                ObjectGUID = SystemObjectModel?.GUID,
                                                PropertyID = model?.ID,
                                                PropertyNum = model?.Num,
                                                Value = model?.Value,
                                                ValueNum = model?.ValueNum
                                            }
                                            );
        }

        private async void EditObject(object param)
        {
            var item = param as SystemObjectPropertyValueModel;
            var items = await Api.GetDataFromServerAsync<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = item.SourceObjectParentGUID });
            var action = await App.Current.MainPage.DisplayActionSheet(null, null, null, items.Select(x => x.Name).ToArray());
            if(action != null)
            {
                using (UserDialogs.Instance.Loading("Внесение изменений...", null, null, true, MaskType.Black))
                {
                    switch (await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue", 
                        new
                        {
                            ObjectGUID = item.SystemObjectGUID,
                            PropertyID = item.ID,
                            PropertyNum = item.Num,
                            Value = items.Where(x => x.Name == action).Select(q => q.GUID).FirstOrDefault(),
                            ValueNum = item.ValueNum
                        })
                    )
                    {
                        case HttpStatusCode.Forbidden: await App.Current.MainPage.DisplayAlert(null, "Нет доступа.", "Oк");break;
                        case HttpStatusCode.BadRequest: await App.Current.MainPage.DisplayAlert(null, "Не удалось редактировать", "Oк"); break;
                            default: Source = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels()); break;
                    };
                }
                 
            }
            
        }

        private async void DeleteObjectProperty(object commandParameter)
        {
            using(UserDialogs.Instance.Loading("Удаление...", null, null, true, MaskType.Black))
            await Task.Run(() => {
                var property = commandParameter as SystemObjectPropertyValueModel;
                Api.SendDataToServer("System/AddSystemObjectPropertyValue", new { ObjectGUID = property.SystemObjectGUID, PropertyID = property.ID, PropertyNum = property.Num, Value = default(string), ValueNum = property.ValueNum });
                Source = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels());
            });
            
        }

        private async Task CreateArrangement(SystemObjectPropertyValueModel property)
        {
            var typeName = Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes", new { ShowHidden = true})
                .Where(x => x.ID == property.SourceObjectTypeID).Select(n => n.Name).FirstOrDefault();
            
            string guidNewItem = await Api.AddSystemObjectAsync("System/AddSystemObject", new { Name = typeName, TypeID = property.SourceObjectTypeID, ParentGUID = property.SystemObjectGUID });
            if (guidNewItem != default(string))
            {
                int valueNum = 0;
                if (property.Value != null)
                {
                    var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                    valueNum = v.Max(x => x.ValueNum);
                }

                property.Value = guidNewItem;
                if(await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                    new
                    {
                        ObjectGUID = SystemObjectModel?.GUID,
                        PropertyID = property.ID,
                        PropertyNum = property.Num,
                        Value = property.Value,
                        ValueNum = valueNum + 1
                    }) == HttpStatusCode.OK)
                {
                    Source = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels());
                }
                
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
                                string guidNewItem = await Api.AddSystemObjectAsync("System/AddSystemObject", new { Name = pResult.Text, TypeID = typeId, ParentGUID = prop.SystemObjectGUID });
                                if (guidNewItem != default(string))
                                {
                                    int valueNum = 0;
                                    if (prop.Value != null && prop.Array == true)
                                    {
                                        var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                                        valueNum = v.Max(x => x.ValueNum);
                                    }
                                    else
                                    {
                                        valueNum = prop.ValueNum;
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
                            if (prop.Value != null && prop.Array == true)
                            {
                                var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                                valueNum = v.Max(x => x.ValueNum);
                            }
                            else
                            {
                                valueNum = prop.ValueNum;
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
        }
       
        /// <summary>
        /// Обновляет источник данных
        /// </summary>
        private async Task<DataSource> UpdateSystemPropertyModels()
        {            
            var SystemPropertyModels = new ObservableCollection<SystemObjectPropertyValueModel>();

            var values = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            var SourceObjects = new DataSource();
            SourceObjects.GroupDescriptors.Add(new GroupDescriptor("GroupName"));

            try
            {                
                foreach (var s in values.OrderByDescending(q => q.TypeID).OrderBy(o => o.GroupID))
                {
                    SystemPropertyModels.Add(s);
                }

                foreach (int? groupId in values.Select(x => x.GroupID))
                {                    
                    if (SystemPropertyModels.LastOrDefault(x => x.TypeID == 5 && !string.IsNullOrEmpty(x.Value as string) && x.GroupID == groupId) != null)
                    {
                        for (int i = SystemPropertyModels.Count - 1; i >= 0; i--)
                        {
                            if (SystemPropertyModels[i].TypeID == 5 && string.IsNullOrEmpty(SystemPropertyModels[i].Value as string) && SystemPropertyModels[i].GroupID == groupId)
                            {
                                SystemPropertyModels.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        for (int j = SystemPropertyModels.Count - 1; j >= 0; j--)
                        {
                            if (SystemPropertyModels[j].TypeID == 5 && string.IsNullOrEmpty(SystemPropertyModels[j].Value as string) && SystemPropertyModels[j].ValueNum > 1 && SystemPropertyModels[j].GroupID == groupId)
                            {
                                SystemPropertyModels.RemoveAt(j);
                            }
                        }
                    }
                }

                SourceObjects.Source = SystemPropertyModels;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, "In foreach (var s in groups){} ->", ex.Message);
            }

            States = States.Normal;
            return SourceObjects;
        }
        
        /// <summary>
        /// Отправляет изменения на сервер
        /// </summary>
        public async void SavePropertiesValuesAsync()
        {
            IsEnabled = false;            
            
            using (UserDialogs.Instance.Loading("Сохранение изменений...", null, null, true, MaskType.Black))
            {
                foreach (var n in NewValues)
                {
                    if (await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue", n) != HttpStatusCode.OK)
                    {
                        UserDialogs.Instance.Alert("Не удалось сохранить", null, "Ок");
                        return;
                    }
                }
                IsVisibleSaveButton = false;
                IsEnabled = true;
                NewValues.Clear();

                await Navigation.PopAsync();
                //Source = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels());
            }
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
           
            if (NewValues.Any(x => x.PropertyID == item.PropertyID))
            {
                for (int i = NewValues.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrEmpty(item.Value as string))
                    {
                        NewValues.Remove(NewValues[i]);
                        break;
                    }
                    else
                    {
                        NewValues[i] = item;
                        break;
                    }
                }
            }
            else if(!string.IsNullOrEmpty(item.Value as string))
            {
                NewValues.Add(item);
            }

            
            if (NewValues.Count > 0)
            {
                IsVisibleSaveButton = true;
            }
            else
            {
                IsVisibleSaveButton = false;
            }
            
        }

        public async Task SaveDate(SystemObjectPropertyValueModel property)
        {
            var item = new AddSystemObjectPropertyValueModel
            {
                ObjectGUID = SystemObjectModel?.GUID,
                PropertyID = property?.ID,
                PropertyNum = property?.Num,
                Value = property?.Value,
                ValueNum = property.ValueNum
            };

            if (await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue", item) != HttpStatusCode.OK)
            {
                UserDialogs.Instance.Alert("Не удалось сохранить", null, "Ок");
                return;
            }
        }
    }
}
