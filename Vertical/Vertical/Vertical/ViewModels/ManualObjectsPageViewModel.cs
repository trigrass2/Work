using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        /// <summary>
        /// папка/файл
        /// </summary>
        public ImageSource ObjectImage { get; set; }

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

        public ManualObjectsPageViewModel(SystemObjectModel _parentObject)
        {
            //ObjectImage = SvgImageSource.FromSvgResource("Vertical.SvgPictures.folder.svg");
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
            
            var items = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = ParentObject?.GUID });            

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
                        await Navigation.PushModalAsync(new EditObjectPage(commandParameter as SystemObjectModel));
                        IsEnabled = true;
                    }
                    break;
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

            await Navigation.PushModalAsync(new InitializeObjectPage(ParentObject));

            IsEnabled = true;
        }

        private async void NextPage(SystemObjectModel _selectedObject)
        {            
            IsEnabled = false;
            States = States.Loading;            
            if(_selectedObject.TypeID == 2)
            {
                await Navigation.PushAsync(await Task.Run(() => new CheckListPage(_selectedObject)));
            }
            else
            {
                await Navigation.PushAsync(await Task.Run(() => new ManualObjectsPage(_selectedObject)));
            }            

            States = States.Normal;
            IsEnabled = true;
        }
    }
}
