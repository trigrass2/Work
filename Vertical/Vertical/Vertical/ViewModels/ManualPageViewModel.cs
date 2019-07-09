using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{    
    /// <summary>
    /// Класс взаимодействия со справочником
    /// </summary>
    public class ManualPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
                
        public ObservableCollection<SystemObjectModel> SystemObjectModels { get; set; }
        public ObservableCollection<SystemObjectTypeModel> SystemObjectTypesModels { get; set; }

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;
        

        /// <summary>
        /// Обновляет содержимое страницы
        /// </summary>
        public ICommand UpdateContentCommand => new Command(UpdateSystemObjects);

        public ICommand GoToAddNewObjectPageCommand => new Command(GoToAddNewObjectPage);
        public ICommand GoToEditObjectPageCommand => new Command(GoToEditObjectPage);

        private delegate IList<T> GetObjectsDelegate<T>();

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
                else _title = "Каталог";
            }
        }        
        public bool IsVisibleTitle { get; set; }

        /// <summary>
        /// вкл/выкл кнопки
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public SystemObjectModel ParentObject { get; set; }

        public ManualPageViewModel(SystemObjectModel _parentObject)
        {            
            ParentObject = _parentObject;
            Title = ParentObject?.Name;
            SystemObjectModels = new ObservableCollection<SystemObjectModel>();
            SystemObjectTypesModels = new ObservableCollection<SystemObjectTypeModel>();
            UpdateSystemObjects();
        }

        /// <summary>
        /// Обновляет коллекцию объектов входной коллекции
        /// </summary>
        /// <typeparam name="T">тип элементов коллекции</typeparam>
        /// <param name="listObjects">обновляемая коллекция</param>
        /// <param name="getObjectsApiFunc">функция для получения нового списка объектов</param>
        public void UpdateSystemObjects()
        {
            if (!NetworkCheck.IsInternet())
            {
                States = States.NoInternet;
                return;
            }

            SystemObjectModels?.Clear();
            
            IList<SystemObjectModel> items = Api.GetDataFromServer<SystemObjectModel>("GetSystemObjects", new { ParentGUID = ParentObject?.GUID });


           if(items != null)
            {
                foreach (var i in items)
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
            string action = await Application.Current.MainPage.DisplayActionSheet("Выберите действие", "Отмена", null, "Редактировать", "Информация");
            
            switch (action)
            {
                case null:
                    {
                        IsEnabled = true;
                        return;
                    }
                case "Редактировать":
                    {
                        await Navigation.PushModalAsync(new InitializeObjectPage(IsAddOrEdit.Edit, commandParameter as SystemObjectModel));
                    }break;
                case "Информация":
                    {
                        await Navigation.PushModalAsync(new InfoPage(commandParameter as SystemObjectModel));
                        IsEnabled = true;
                    }break;
            }

            IsEnabled = true;
        }
        
        private async void GoToAddNewObjectPage()
        {
            IsEnabled = false;

            await Navigation.PushModalAsync(new InitializeObjectPage(IsAddOrEdit.Add,ParentObject));

            IsEnabled = true;
        }

        private async void NextPage(SystemObjectModel _selectedObject)
        {            
            IsEnabled = false;
            States = States.Loading;            
            
            await Navigation.PushAsync(await Task.Run(()=> new ManualPage(_selectedObject)));

            States = States.Normal;
            IsEnabled = true;
        }
    }
}
