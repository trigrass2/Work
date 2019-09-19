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
        public NotifyTaskCompletion<DataSource> Source { get; set; }
        public DataSource SourceObjects { get; set; }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime SelectedDate { get; set; }
                        
        public CheckPageViewModel() { }

        public CheckPageViewModel(SystemObjectModel obj)
        {
            SystemPropertyModels = new ObservableCollection<SystemObjectPropertyValueModel>();
            MinDate = DateTime.Now.AddYears(-1);
            MaxDate = DateTime.Now.AddYears(1);
            SystemObjectModel = obj;
                        
            NewValues = new List<AddSystemObjectPropertyValueModel>();
            Source = new NotifyTaskCompletion<DataSource>(UpdateSystemPropertyModels());
        }

        private async void IsChecked(object obj)
        {
            if (!NetworkCheck.IsInternet())
            {
                await UserDialogs.Instance.AlertAsync("Нет подключения к интернету");
                return;
            }
            if(obj is SystemObjectPropertyValueModel model)
            {
                await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
                                            new
                                            {
                                                ObjectGUID = SystemObjectModel?.GUID,
                                                PropertyID = model?.ID,
                                                PropertyNum = model?.Num,
                                                model?.Value,
                                                model?.ValueNum
                                            }
                                            );
            }
            
        }

        private async void EditObject(object param)
        {
            var item = new SystemObjectPropertyValueModel(param as SystemObjectPropertyValueModel);
            var items = await Api.GetDataFromServerAsync<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = item.SourceObjectParentGUID });
            
            var action = await UserDialogs.Instance.ActionSheetAsync(null, "Отмена", null, buttons: items.Select(x => x.Name).ToArray());
            if(action != null)
            {
                item.Value = items.Where(x => x.Name == action).Select(q => q.GUID).FirstOrDefault();
                int index = SystemPropertyModels.Where(x => x.ID == item.ID).Select(x => SystemPropertyModels.IndexOf(x)).Single();
                SystemPropertyModels[index].Value = item.Value;
                CreateNewValue(item, item.Value);
            }
        }

        private void DeleteObjectProperty(object commandParameter)
        {
            var property = commandParameter as SystemObjectPropertyValueModel;
            SystemPropertyModels.Remove(property);
        }
                
        private async Task CreateArrangement(SystemObjectPropertyValueModel property)
        {
            var newProperty = new SystemObjectPropertyValueModel(property);
            var typeName = await Api.GetDataFromServerAsync<SystemObjectTypeModel>("System/GetSystemObjectTypes", new { ShowHidden = true });                
            
            string guidNewItem = await Api.AddSystemObjectAsync("System/AddSystemObject", 
                                                                new {
                                                                    Name = typeName.Where(x => x.ID == property.SourceObjectTypeID).Select(n => n.Name).FirstOrDefault(),
                                                                    TypeID = property.SourceObjectTypeID,
                                                                    ParentGUID = property.SystemObjectGUID });
            if (guidNewItem != default)
            {
                int valueNum = 0;
                if (newProperty.Value != null)
                {                    
                    valueNum = SystemPropertyModels.Where(q => q?.ID == newProperty?.ID).Max(x => x.ValueNum);
                }
                newProperty.ValueNum = valueNum + 1;
                newProperty.Value = guidNewItem;
                SystemPropertyModels.Add(newProperty);
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

            //else
            //{
            //    //if (string.IsNullOrEmpty(prop.SourceObjectParentGUID))
            //    //{
            //    //    var types = await Api.GetDataFromServerAsync<SystemObjectTypeModel>("System/GetSystemObjectTypes");
            //    //    var action = await UserDialogs.Instance.ActionSheetAsync(
            //    //                                  "Тип нового объекта",
            //    //                                  "Отмена",
            //    //                                  null,
            //    //                                  buttons: types.Select(x => x.Name).ToArray());
            //    //    if (!string.IsNullOrEmpty(action) && action != "отмена")
            //    //    {
            //    //        int typeId = types.Where(x => x.Name == action).Select(x => x.ID).FirstOrDefault();
            //    //        PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            //    //        {
            //    //            InputType = InputType.Name,
            //    //            OkText = "Создать",
            //    //            Title = "Создание объекта"
            //    //        });
            //    //        using (UserDialogs.Instance.Loading("Создание объекта...", null, null, true, MaskType.Black))
            //    //        {
            //    //            if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
            //    //            {
            //    //                string guidNewItem = await Api.AddSystemObjectAsync("System/AddSystemObject", new { Name = pResult.Text, TypeID = typeId, ParentGUID = prop.SystemObjectGUID });
            //    //                if (guidNewItem != default(string))
            //    //                {
            //    //                    int valueNum = 0;
            //    //                    if (prop.Value != null && prop.Array == true)
            //    //                    {
            //    //                        var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            //    //                        valueNum = v.Max(x => x.ValueNum);
            //    //                    }
            //    //                    else
            //    //                    {
            //    //                        valueNum = prop.ValueNum;
            //    //                    }

            //    //                    prop.Value = guidNewItem;
            //    //                    await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
            //    //                        new
            //    //                        {
            //    //                            ObjectGUID = SystemObjectModel?.GUID,
            //    //                            PropertyID = prop.ID,
            //    //                            PropertyNum = prop.Num,
            //    //                            Value = prop.Value,
            //    //                            ValueNum = valueNum + 1
            //    //                        });

            //    //                    //Source = new NotifyTaskCompletion<ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels());
            //    //                }

            //    //            }
            //    //        }

            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    var objects = await Api.GetDataFromServerAsync<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = prop.SourceObjectParentGUID });
            //    //    var action = await UserDialogs.Instance.ActionSheetAsync(
            //    //                                      "",
            //    //                                      "Отмена",
            //    //                                      null,
            //    //                                      buttons: objects.Select(x => x.Name).ToArray());
            //    //    if (!string.IsNullOrEmpty(action) && action != "отмена")
            //    //    {
            //    //        using (UserDialogs.Instance.Loading("Создание объекта...", null, null, true, MaskType.Black))
            //    //        {
            //    //            var item = objects.Where(x => x.Name == action).FirstOrDefault();
            //    //            int valueNum = 0;
            //    //            if (prop.Value != null && prop.Array == true)
            //    //            {
            //    //                var v = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });
            //    //                valueNum = v.Max(x => x.ValueNum);
            //    //            }
            //    //            else
            //    //            {
            //    //                valueNum = prop.ValueNum;
            //    //            }
            //    //            prop.Value = item.GUID;
            //    //            await Api.SendDataToServerAsync("System/AddSystemObjectPropertyValue",
            //    //                         new
            //    //                         {
            //    //                             ObjectGUID = SystemObjectModel?.GUID,
            //    //                             PropertyID = prop.ID,
            //    //                             PropertyNum = prop.Num,
            //    //                             Value = prop.Value,
            //    //                             ValueNum = valueNum + 1
            //    //                         });

            //    //            //Source = new NotifyTaskCompletion<ObservableCollection<IGrouping<string, SystemObjectPropertyValueModel>>>(UpdateSystemPropertyModels());
            //    //        }
            //    //    }
            //    //}
            //}
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
        private async Task<DataSource> UpdateSystemPropertyModels()
        {
            States = States.Loading;
            SystemPropertyModels.Clear();
            var values = await Api.GetDataFromServerAsync<SystemObjectPropertyValueModel>("System/GetSystemObjectPropertiesValues", new { ObjectGUID = SystemObjectModel?.GUID });

            SourceObjects = new DataSource();
            SourceObjects.GroupDescriptors.Add(new GroupDescriptor("GroupName"));

            try
            {                
                foreach (var s in values.OrderByDescending(q => q.TypeID).OrderBy(f => f.GroupID))
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
 
            SystemPropertyModels.CollectionChanged += UpdateSource;
            States = States.Normal;
            SourceObjects.Source = SystemPropertyModels;
            
            return SourceObjects;            
        }

        private async void UpdateSource(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                SystemObjectPropertyValueModel item = new SystemObjectPropertyValueModel();
                if (e.OldItems != null)
                {
                    item = e.OldItems[0] as SystemObjectPropertyValueModel;
                    item.Value = null;
                }
                else
                {
                    item = e.NewItems[0] as SystemObjectPropertyValueModel;
                }

                CreateNewValue(item, item?.Value);
            }
            catch (Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
            }            
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
                if(prop.ValueNum == item.ValueNum)
                {
                    NewValues[NewValues.IndexOf(prop)] = item;
                }
                else
                {
                    NewValues.Add(item);
                }
                
            }
            else
            {
                NewValues.Add(item);
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
