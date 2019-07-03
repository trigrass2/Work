using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    /// <summary>
    /// Логика создания заявки
    /// </summary>
    public class CreateTaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CreateTaskModel NewTask { get; set; }

        /// <summary>
        /// Отправляет новую заявку
        /// </summary>
        public ICommand SendTaskCommand { get; set; }
        public ICommand GetFileCommand { get; set; }
        public INavigation Navigation { get; set; }

        private FileData file = new FileData();

        private ServiceDesk_TypeListView _selectedType;
        public ServiceDesk_TypeListView SelectedTypes
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                NewTask.Type_id = _selectedType?.Type_id ?? null;
            }
        }
        private Product_PlantListView _selectedPlant;
        public Product_PlantListView SelectedPlant
        {
            get
            {
                return _selectedPlant;
            }
            set
            {
                _selectedPlant = value;
                NewTask.Plant_id = _selectedPlant?.Plant_id ?? null;
                UpdateUnits(NewTask?.Plant_id, NewTask?.Factory_id);
            }
        }
        private Product_UnitListView _selectedUnit;
        public Product_UnitListView SelectedUnit
        {
            get
            {
                return _selectedUnit;
            }
            set
            {
                _selectedUnit = value;
                NewTask.Unit_id = _selectedUnit?.Unit_id ?? null;
            }
        }
        private Product_FactoryListView _selectedFactory;
        public Product_FactoryListView SelectedFactory
        {
            get
            {
                return _selectedFactory;
            }
            set
            {
                _selectedFactory = value;
                NewTask.Factory_id = _selectedFactory?.Factory_id ?? null;
                UpdatePlants(NewTask.Factory_id);
            }
        }
        private string _selectedUser;
        public string SelectedRecipent
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                NewTask.Recipient_id = Users.Where(x => x.UserName == _selectedUser).Select(x => x.Id).FirstOrDefault();
            }
        }

        public ObservableCollection<Product_FactoryListView> Factorys { get; set; }
        public ObservableCollection<ServiceDesk_TypeListView> Types { get; set; }
        public ObservableCollection<Product_PlantListView> Plants { get; set; }
        public ObservableCollection<Product_UnitListView> Units { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> UsersNames { get; set; }

        public bool _initializedTypes = false;
        public bool _initializedFactorys = false;
        public bool _initializedUsers = false;

        

        public CreateTaskViewModel()
        {
            
            Factorys = new ObservableCollection<Product_FactoryListView>();
            Types = new ObservableCollection<ServiceDesk_TypeListView>();
            Plants = new ObservableCollection<Product_PlantListView>();
            Units = new ObservableCollection<Product_UnitListView>();
            Users = new ObservableCollection<UserModel>();
            UsersNames = new ObservableCollection<string>();

            NewTask = new CreateTaskModel() { Attachments = new ObservableCollection<AttachmentFileModel>() };
            SendTaskCommand = new Command(SendTask);
            GetFileCommand = new Command(GetFile);
            UpdatePlants(NewTask?.Factory_id);
            UpdateUnits(NewTask?.Plant_id, NewTask?.Factory_id);
        }

        /// <summary>
        /// отправляет данные
        /// </summary>
        public async void SendTask()
        {
            try
            {
                Log.WriteMessage("Создание новой заявки...");
                
                if(NewTask.Type_id == null || NewTask.Title == null || NewTask.Text == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Заполните обязательные поля! (Тип заявки, Заголовок заявки, Текст заявки)", "OK");
                    return;
                }

                await ServiceDeskApi.SendDataToServerAsync(NewTask, ServiceDeskApi.ApiEnum.CreateTask);
                Log.WriteMessage("Заявка создана");
                
                LoadPage loadPage = new LoadPage();               

                NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                MenuPage homePage = navStack[navPage.Navigation.NavigationStack.Count - 2] as MenuPage;
                SendTaskPage thisPage = navStack[navPage.Navigation.NavigationStack.Count - 1] as SendTaskPage;

                await Navigation.PushAsync(loadPage);

                if (homePage != null)
                {
                    await Task.Run(() =>
                    {                        
                        homePage.ViewModel.UpdateTasks(homePage.ViewModel.Filter.Status_id);                        
                    });                    
                }
                
                Navigation.RemovePage(loadPage);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при создании заявки {ex.Message}");
            }
            
        }

        /// <summary>
        /// полyчает данные выбранного файла
        /// </summary>
        public async void GetFile()
        {
            try
            {
                Log.WriteMessage($"Открытие файла...");

                file = await CrossFilePicker.Current.PickFile();

                if (file != null)
                {
                    NewTask.Attachments.Add(new AttachmentFileModel { Attachment_name = file.FileName, Attachment_bytes = file.DataArray });
                }

                Log.WriteMessage("Файл добавлен");
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при открытии файла {ex.Message}");
            }
            
        }

        #region UPDATE DATA

        public async Task UpdateUsers()
        {
            if (_initializedUsers == true) return;

            Users.Clear();
            var users = await ServiceDeskApi.GetAllUsersAsync(new { User_id = default(string), Search = default(string) }, ServiceDeskApi.ApiEnum.GetUsersList);
            Users = new ObservableCollection<UserModel>(users);
            UsersNames = new ObservableCollection<string>(users.Select(x => x.UserName));

            _initializedUsers = true;
        }

        /// <summary>
        /// Обновляет типы заявок
        /// </summary>
        public async Task UpdateTypes()
        {
            if (_initializedTypes == true) return;
            
            var typesTasks = await ServiceDeskApi.GetDataServisDeskManagmentAsync<ServiceDesk_TypeListView>(ServiceDeskApi.ApiEnum.GetTypes);
            Types.Clear();
            foreach(var t in typesTasks)
            {
                Types.Add(t);
            }
            
            _initializedTypes = true;
        }

        /// <summary>
        /// Обновляет список заводов
        /// </summary>
        public async Task UpdateFactorys()
        {
            if (_initializedFactorys == true) return;
            Factorys.Clear();

            var factorys = await ServiceDeskApi.GetProductUnitAsync<Product_FactoryListView>(ServiceDeskApi.ApiEnum.GetProductFactoryList);
            foreach (var f in factorys)
            {
                Factorys.Add(f);
            }
            
            _initializedFactorys = true;
        }

        /// <summary>
        /// Обновляет линии
        /// </summary>
        private void UpdatePlants(int? idFactory)
        {
            Plants.Clear();

            var plants = ServiceDeskApi.GetProductUnit<Product_PlantListView>(ServiceDeskApi.ApiEnum.GetProductPlantList, idFactory);

            foreach (var p in plants)
            {
                Plants.Add(p);
            }
        }

        /// <summary>
        /// Обновляет юниты
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="plantId"></param>
        private void UpdateUnits(int? idPlant, int? idFactory)
        {
            Units.Clear();

            var units = ServiceDeskApi.GetProductUnit<Product_UnitListView>(ServiceDeskApi.ApiEnum.GetProductUnitList, idPlant, idFactory);

            foreach (var u in units)
            {
                Units.Add(u);
            }

        }
        #endregion

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
