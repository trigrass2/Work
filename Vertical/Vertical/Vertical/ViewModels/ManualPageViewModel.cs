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
    public class ManualPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
                
        public ObservableCollection<SystemObjectModel> SystemObjectModels { get; set; }
        public ObservableCollection<SystemObjectTypeModel> SystemObjectTypesModels { get; set; }

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.NoData;
        public ICommand AddNewObjectCommand => new Command(GoToAddNewObjectPage);
        public ICommand UpdateContentCommand => new Command(UpdateSystemObjects);
        public ICommand EditObjectCommand => new Command(GoToEditObjectPage);
        public ImageSource EditButtonImage { get; set; }

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
                if(_title != value)
                {
                    _title = value;
                    IsVisibleTitle = string.IsNullOrEmpty(_title) ? false : true;
                }
            }
        }        
        public bool IsVisibleTitle { get; set; }
        public bool IsEnabled { get; set; } = true;

        public SystemObjectModel ParentObject { get; set; }

        public ManualPageViewModel(SystemObjectModel _parentObject)
        {
            EditButtonImage = SvgImageSource.FromSvgResource("Vertical.SvgPictures.ic_edit.svg", 20,20);
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
            
            var items = Api.GetDataFromServer<SystemObjectModel>("GetSystemObjects", new { ParentGUID = ParentObject?.GUID });

            foreach (var i in items)
            {
                SystemObjectModels.Add(i);
            }

           // States = SystemObjectModels.Count == 0 ? States.NoData : States.Normal;
        }

        private async void GoToEditObjectPage(object commandParameter)
        {
            IsEnabled = false;

            await Navigation.PushModalAsync(new InitializeObjectPage(IsAddOrEdit.Edit,commandParameter as SystemObjectModel));

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
