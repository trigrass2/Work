using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
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
                if (_selectedPlant != null && Units != null)
                    UpdateUnits(_selectedFactory.Factory_id, _selectedPlant.Plant_id);
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
                
                if (_selectedFactory != null && Plants != null)
                    UpdatePlants(_selectedFactory.Factory_id);
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
            }
        }

        public ObservableCollection<Product_FactoryListView> Factorys { get; set; }        
        public ObservableCollection<ServiceDesk_TypeListView> Types { get; set; }      
        public ObservableCollection<Product_PlantListView> Plants { get; set; }
        public ObservableCollection<Product_UnitListView> Units { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> UsersNames { get; set; }

        #region set model

        public string Title
        {
            get
            {
                return NewTask.Title;
            }
            set
            {


                NewTask.Title = value;

            }
        }

        public string Text
        {
            get
            {
                return NewTask.Text;
            }
            set
            {


                NewTask.Text = value;

            }
        }

        public ObservableCollection<AttachmentFileModel> Attachments
        {
            get
            {
                return NewTask.Attachments;
            }
            set
            {
                NewTask.Attachments = value;
            }

        }

        
        #endregion

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
        }

        /// <summary>
        /// отправляет данные
        /// </summary>
        public async void SendTask()
        {
            NewTask.Type_id = _selectedType?.Type_id ?? null;
            NewTask.Factory_id = _selectedFactory?.Factory_id ?? null;
            NewTask.Plant_id = _selectedPlant?.Plant_id ?? null;
            NewTask.Unit_id = _selectedUnit?.Unit_id ?? null;
            NewTask.Recipient_id = Users.Where(x => x.UserName == _selectedUser).Select(x => x.Id).FirstOrDefault();
            await ServiceDeskApi.SendDataToServerAsync(NewTask, ServiceDeskApi.ApiEnum.CreateTask);
            await Navigation.PopAsync();
        }

        /// <summary>
        /// полyчает данные выбранного файла
        /// </summary>
        public async void GetFile()
        {
            
            file = await CrossFilePicker.Current.PickFile();            

            if (file != null)
            {
                NewTask.Attachments.Add(new AttachmentFileModel { Attachment_name = file.FileName, Attachment_bytes = file.DataArray });
            }
        }

        #region UPDATE DATA

        public async Task UpdateUsers()
        {
            Users.Clear();
            var users = await ServiceDeskApi.GetAllUsersAsync(new { User_id = default(string), Search = default(string) }, ServiceDeskApi.ApiEnum.GetUsersList);
            Users = new ObservableCollection<UserModel>(users);
            UsersNames = new ObservableCollection<string>(users.Select(x => x.UserName));
        }

        /// <summary>
        /// Обновляет типы заявок
        /// </summary>
        public async Task UpdateTypes()
        {            
            var typesTasks = await ServiceDeskApi.GetDataServisDeskManagmentAsync<ServiceDesk_TypeListView>(ServiceDeskApi.ApiEnum.GetTypes);
            Types.Clear();
            foreach(var t in typesTasks)
            {
                Types.Add(t);
            }
        }

        /// <summary>
        /// Обновляет список заводов
        /// </summary>
        public async Task UpdateFactorys()
        {
            Factorys.Clear();

            var factorys = await ServiceDeskApi.GetProductUnitAsync<Product_FactoryListView>(ServiceDeskApi.ApiEnum.GetProductFactoryList);
            foreach (var f in factorys)
            {
                Factorys.Add(f);
            }

           
        }

        /// <summary>
        /// Обновляет линии
        /// </summary>
        private void UpdatePlants(int factoryId)
        {
            Plants.Clear();

            if(factoryId > 0)
            {
                var plants = ServiceDeskApi.GetProductUnit<Product_PlantListView>(ServiceDeskApi.ApiEnum.GetProductPlantList, factoryId);
                foreach (var p in plants)
                {
                    Plants.Add(p);
                }
            }
            
        }

        /// <summary>
        /// Обновляет юниты
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="plantId"></param>
        private void UpdateUnits(int factoryId, int plantId)
        {
            Units.Clear();

            if (factoryId > 0 && plantId > 0)
            {
                var units = ServiceDeskApi.GetProductUnit<Product_UnitListView>(ServiceDeskApi.ApiEnum.GetProductUnitList, factoryId, plantId);
                foreach (var u in units)
                {
                    Units.Add(u);
                }
            }

        }
        #endregion
    }
}
