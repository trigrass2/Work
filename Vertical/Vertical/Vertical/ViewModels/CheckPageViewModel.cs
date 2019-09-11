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
using System.Collections.Specialized;

namespace Vertical.ViewModels
{    
    public class CheckPageViewModel : BaseViewModel
    {        
        public ICommand AddNewObjectInPropertyCommand => new Command(AddNewObjectInPropperty);
        public ICommand EditObjectCommand => new Command(EditObject);
        public ICommand IsCheckedCommand => new Command(IsChecked);
        public ICommand DeletePropertyCommand => new Command(DeleteObjectProperty);

        private ObservableCollection<SystemObjectPropertyValueModel> SystemPropertyModels { get; set; }
        public SystemObjectModel SystemObjectModel { get; set; }
        public List<AddSystemObjectPropertyValueModel> NewValues { get; set; }
        public NotifyTaskCompletion<ObservableCollection<Grouping<string, SystemObjectPropertyValueModel>>> Source { get; set; }
        public DataSource SourceObjects { get; set; }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }

        private int? PropertyID { get; set; }
        public bool IsChanged { get; set; } = false;

        public CheckPageViewModel() { }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemPropertyModels = new ObservableCollection<SystemObjectPropertyValueModel>();
            MinDate = DateTime.Now.AddYears(-1);
            MaxDate = DateTime.Now.AddYears(1);
            SystemObjectModel = obj;

            NewValues = new List<AddSystemObjectPropertyValueModel>();
            Source = new NotifyTaskCompletion<ObservableCollection<Grouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels());
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
                            //default: Source = new NotifyTaskCompletion<ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels()); break;
                    };
                }
                 
            }
            
        }

        private async void DeleteObjectProperty(object commandParameter)
        {
            using(UserDialogs.Instance.Loading("Удаление...", null, null, true, MaskType.Black))
            {
                await Task.Run(() => {
                    var property = commandParameter as SystemObjectPropertyValueModel;
                    
                    //Api.SendDataToServer("System/AddSystemObjectPropertyValue", new { ObjectGUID = property.SystemObjectGUID, PropertyID = property.ID, PropertyNum = property.Num, Value = default(string), ValueNum = property.ValueNum });
                    //Source = new NotifyTaskCompletion<ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels());
                });
            }            
            
        }

        private async Task CreateArrangement(SystemObjectPropertyValueModel property)
        {
            var typeName = await Api.GetDataFromServerAsync<SystemObjectTypeModel>("System/GetSystemObjectTypes", new { ShowHidden = true });                
            
            string guidNewItem = await Api.AddSystemObjectAsync("System/AddSystemObject", 
                                                                new {
                                                                    Name = typeName.Where(x => x.ID == property.SourceObjectTypeID).Select(n => n.Name).FirstOrDefault(),
                                                                    TypeID = property.SourceObjectTypeID,
                                                                    ParentGUID = property.SystemObjectGUID });
            if (guidNewItem != default)
            {
                int valueNum = 0;
                if (property.Value != null)
                {
                    var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
                    valueNum = v.Where(q => q?.ID == property?.ID).Max(x => x.ValueNum);
                }
                property.ValueNum = valueNum + 1;
                SystemPropertyModels.Add(property);                
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

                                    //Source = new NotifyTaskCompletion<ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels());
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

                            //Source = new NotifyTaskCompletion<ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels());
                        }
                    }
                }
            }
        }

        private async Task DeleteNullValues(IEnumerable<SystemObjectPropertyValueModel> values, int? groupId)
        {
            await Task.Run(()=> {
                if (SystemPropertyModels.LastOrDefault(x => x.TypeID == 5 && !string.IsNullOrEmpty(x.Value as string) && x.GroupID == groupId) != null)
                {
                    for (int i = SystemPropertyModels.Count - 1; i >= 0; i--)
                    {
                        if (SystemPropertyModels[i].GroupID == groupId && SystemPropertyModels[i].TypeID == 5 && string.IsNullOrEmpty(SystemPropertyModels[i].Value as string))
                        {
                            SystemPropertyModels.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    for (int j = SystemPropertyModels.Count - 1; j >= 0; j--)
                    {
                        if (SystemPropertyModels[j].GroupID == groupId && SystemPropertyModels[j].TypeID == 5 && string.IsNullOrEmpty(SystemPropertyModels[j].Value as string) && SystemPropertyModels[j].ValueNum > 1)
                        {
                            SystemPropertyModels.RemoveAt(j);
                        }
                    }
                }
                //if (values.LastOrDefault(x => x.TypeID == 5 && !string.IsNullOrEmpty(x.Value as string) && x.GroupID == groupId) != null)
                //{
                //    for (int i = DictionaryProperties.Values.Count() - 1; i >= 0; i--)
                //    {
                //        var item = DictionaryProperties.Values.ElementAt(i);
                //        for (int j = item.Count - 1; j >= 0; j--)
                //        {
                //            if (item[j].TypeID == 5 && string.IsNullOrEmpty(item[j].Value as string) && item[j].GroupID == groupId)
                //            {
                //                item.RemoveAt(j);
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    for (int k = DictionaryProperties.Values.Count() - 1; k >= 0; k--)
                //    {
                //        var item = DictionaryProperties.Values.ElementAt(k);
                //        for (int l = item.Count - 1; l >= 0; l--)
                //        {
                //            if (item[l].TypeID == 5 && string.IsNullOrEmpty(item[l].Value as string) && item[l].ValueNum > 1 && item[l].GroupID == groupId)
                //            {
                //                item.RemoveAt(l);
                //            }
                //        }
                //    }
                //}
            });            
        }

        /// <summary>
        /// Обновляет источник данных
        /// </summary>
        private async Task<ObservableCollection<Grouping<string, SystemObjectPropertyValueModel>>> UpdateSystemPropertyModels()
        {
            States = States.Loading;
            SystemPropertyModels.Clear();
            var values = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            
            try
            {
                //foreach(string groupName in values.OrderByDescending(q => q.TypeID).Select(x => x.GroupName).Distinct())
                //{
                //    DictionaryProperties.Add(
                //        string.IsNullOrEmpty(groupName) ? "" : groupName,
                //        new ObservableCollection<SystemObjectPropertyValueModel>(values.Where(g => g.GroupName == groupName).ToList())
                //        );
                //}

                //foreach (int? groupId in values.Select(x => x.GroupID))
                //{
                //    await DeleteNullValues(values, groupId);
                //}

                foreach (var s in values.OrderByDescending(q => q.TypeID))
                {
                    SystemPropertyModels.Add(s);
                }

                foreach (int? groupId in values.Select(x => x.GroupID))
                {
                    await DeleteNullValues(values, groupId);
                }
            }
            catch (Exception ex)
            {
               await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, "In foreach (var s in groups){} ->", ex.Message);
            }
                        
            //var gr = new ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>(SystemPropertyModels.GroupBy(x => x.GroupName));
            SystemPropertyModels.CollectionChanged += UpdateSource;
            States = States.Normal;
            
            return new ObservableCollection<Grouping<string, SystemObjectPropertyValueModel>>(SystemPropertyModels.GroupBy(x => x.GroupName).Select(g => new Grouping<string, SystemObjectPropertyValueModel>(g.Key, g))); ;
        }

        private void UpdateSource(object sender, NotifyCollectionChangedEventArgs e)
        {            
            var item = (e.NewItems[0] as SystemObjectPropertyValueModel);
            var sourceCollection = Source.Result;

            Source.Result[sourceCollection.IndexOf(sourceCollection.Where(x => x.Key == item?.GroupName).Single())].Values.Add(item);

            CreateNewValue(item, item?.Value);
        }

        /// <summary>
        /// Отправляет изменения на сервер
        /// </summary>
        public async Task SavePropertiesValuesAsync()
        {
            IsEnabled = false;
            
            foreach (var n in NewValues.AsParallel())
            {
                if (await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue", n) != HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Alert("Не удалось сохранить", null, "Ок");
                    return;
                }
            }

            IsEnabled = true;
            NewValues.Clear();
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

            var prop = NewValues.FirstOrDefault(x => x.PropertyID == item?.PropertyID);
            if (prop != null)
            {                
                NewValues[NewValues.IndexOf(prop)] = item;
                IsChanged = true;
            }
            else
            {
                NewValues.Add(item);
                IsChanged = true;
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
