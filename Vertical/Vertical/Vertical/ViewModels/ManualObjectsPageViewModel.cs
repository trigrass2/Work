using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using Xamarin.Forms.Svg;
using static Vertical.Constants;

namespace Vertical.ViewModels
{    
    /// <summary>
    /// Класс взаимодействия со справочником
    /// </summary>
    public class ManualObjectsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
                
        public ObservableCollection<SystemObjectModel> SystemObjectModels { get; set; }         

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Loading;        
        public ICommand UpdateContentCommand => new Command(UpdateSystemObjects);
        public ICommand GoToAddNewObjectPageCommand => new Command(GoToAddNewObjectPage);
        public ICommand GoToEditObjectPageCommand => new Command(GoToEditObjectPage);
        
        private SystemObjectModel _selectedObject;
        public SystemObjectModel SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                var tempObject = value;
                _selectedObject = null;
                NextPage(tempObject);
            }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                }
                else _title = "";
            }
        }
        
        /// <summary>
        /// вкл/выкл кнопки
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public SystemObjectModel ParentObject { get; set; }
        private string TypePage { get; set; }

        public ManualObjectsPageViewModel(SystemObjectModel _parentObject, string typePage)
        {
            TypePage = typePage;
            ParentObject = _parentObject;
            Title = ParentObject?.Name;
            SystemObjectModels = new ObservableCollection<SystemObjectModel>();
            
            UpdateSystemObjects();
        }

        public void UpdateSystemObjects()
        {
            if (!NetworkCheck.IsInternet())
            {
                States = States.NoInternet;
                return;
            }

            SystemObjectModels?.Clear();

            IList<SystemObjectModel> items = default(IList<SystemObjectModel>);

            if(TypePage == "Шаблоны")
            {
               items = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = ParentObject?.GUID, ShowTemplates = true });
            }
            else
            {
                items = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = ParentObject?.GUID});
            }

            if (items != null)
            {
                foreach(var i in items)
                {
                    SystemObjectModels.Add(i);
                }

                States = SystemObjectModels.Count > 0 ? States.Normal : States.NoData;
            }
            else
            {
                States = States.NoAccess;
            }
        }

        private async void GoToEditObjectPage(object commandParameter)
        {            
            IsEnabled = false;
            await Navigation.PushModalAsync(new EditObjectPage(commandParameter as SystemObjectModel));

            IsEnabled = true;
        }
        
        private async void GoToAddNewObjectPage()
        {
            IsEnabled = false;

            await Navigation.PushAsync(new ManualObjectsPage(null,"Архив"));

            IsEnabled = true;
        }

        private async void NextPage(SystemObjectModel _selectedObject)
        {            
            IsEnabled = false;
            States = States.Loading;
            var types = await Api.GetDataFromServerAsync<SystemObjectTypeModel>("System/GetSystemObjectTypes", new { ShowHidden = true });

            try
            {
                if (types.FirstOrDefault(x => x.ID == _selectedObject.TypeID).PrototypeID > 1)
                {
                    if (_selectedObject?.Template == true)
                    {                        
                        string action = await App.Current.MainPage.DisplayActionSheet("Выберите действие", "Отмена", null, "Создать", "Архив");
                        switch (action)
                        {
                            case "Создать":
                                {
                                    try
                                    {
                                        PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                                        {
                                            InputType = InputType.Name,
                                            Text = $"{_selectedObject.Name} {DateTime.Now}",
                                            OkText = "Создать",
                                            Title = "Создание объекта"
                                        });
                                        if (pResult.Ok)
                                        {
                                            string guidNewObject = await Api.AddSystemObjectAsync("System/CloneSystemObject", new { ObjectGUID = _selectedObject?.GUID, Name = pResult?.Text, ParentObject = _selectedObject?.ParentGUID, TypeID = _selectedObject?.TypeID });
                                            var obj = await Api.GetDataFromServerAsync<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = guidNewObject });
                                            await Navigation.PushAsync(await Task.Run(() => new CheckListPage(obj.FirstOrDefault())));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Loger.WriteMessageAsync(Android.Util.LogPriority.Error, "При создании объекта", ex.Message);                                        
                                    }
                                    
                                }
                                break;
                            case "Архив":
                                {
                                    var item = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = _selectedObject.ParentGUID }).FirstOrDefault();
                                    await Navigation.PushAsync(await Task.Run(() => new ManualObjectsPage(
                                        item,
                                        "Архив"))
                                        );
                                }
                                break;
                        }

                    }
                    else
                    {
                        await Navigation.PushAsync(await Task.Run(() => new CheckListPage(_selectedObject)));
                    }
                }
                else
                {
                    await Navigation.PushAsync(await Task.Run(() => new ManualObjectsPage(_selectedObject, "Архив")));
                }
            }
            catch (System.Exception)
            {
                if (_selectedObject?.Template == true)
                {
                    string action = await App.Current.MainPage.DisplayActionSheet("Выберите действие", "Отмена", null, "Создать", "Архив");
                    switch (action)
                    {
                        case "Создать":
                            {
                                PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                                {
                                    InputType = InputType.Name,
                                    Text = $"{_selectedObject.Name} {DateTime.Now}",
                                    OkText = "Создать",
                                    Title = "Создание объекта"
                                });
                                if (pResult.Ok)
                                {
                                    string guidNewObject = await Api.AddSystemObjectAsync("System/CloneSystemObject", new { ObjectGUID = _selectedObject?.GUID, Name = pResult?.Text, ParentObject = _selectedObject?.ParentGUID, TypeID = _selectedObject?.TypeID });
                                    var obj = await Api.GetDataFromServerAsync<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = guidNewObject });
                                    await Navigation.PushAsync(await Task.Run(() => new CheckListPage(obj.FirstOrDefault())));
                                }
                            }
                            break;
                        case "Архив":
                            {

                                await Navigation.PushAsync(await Task.Run(() => new ManualObjectsPage(
                                    Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = _selectedObject.ParentGUID }).FirstOrDefault(),
                                    "Архив"))
                                    );
                            }
                            break;
                    }

                }
                else
                {
                    await Navigation.PushAsync(await Task.Run(() => new CheckListPage(_selectedObject)));
                }
            }
                     

            States = States.Normal;
            IsEnabled = true;
        }
    }
}
