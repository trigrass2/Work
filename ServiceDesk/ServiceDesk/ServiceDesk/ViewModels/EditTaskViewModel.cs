using ServiceDesk.Models;
using ServiceDesk.PikApi;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    public class EditTaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EditTaskModel NewTask { get; set; }

        /// <summary>
        /// Отправляет новую заявку
        /// </summary>
        public ICommand SendTaskCommand { get; set; }
        public INavigation Navigation { get; set; }

        public ServiceDesk_TaskListView TaskEdit { get; set; }

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
                if (_selectedPlant != null)
                {
                    UpdateUnits(_selectedPlant.Plant_id);
                }               
                    
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

                if (_selectedFactory != null)
                {
                    UpdatePlants(_selectedFactory.Factory_id);
                }
                
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

        #endregion

        public EditTaskViewModel(ServiceDesk_TaskListView taskModel)
        {
            TaskEdit = taskModel;
            Factorys = new ObservableCollection<Product_FactoryListView>();
            Types = new ObservableCollection<ServiceDesk_TypeListView>();
            Plants = new ObservableCollection<Product_PlantListView>();
            Units = new ObservableCollection<Product_UnitListView>();
            Users = new ObservableCollection<UserModel>();
            UsersNames = new ObservableCollection<string>();

            NewTask = new EditTaskModel();
            SendTaskCommand = new Command(SendTask);
        }

        /// <summary>
        /// отправляет данные
        /// </summary>
        public async void SendTask()
        {
            NewTask.Task_id = TaskEdit?.Task_id;
            NewTask.Type_id = _selectedType?.Type_id ?? null;
            NewTask.Factory_id = _selectedFactory?.Factory_id ?? null;
            NewTask.Plant_id = _selectedPlant?.Plant_id ?? null;
            NewTask.Unit_id = _selectedUnit?.Unit_id ?? null;
            NewTask.Recipient_id = Users.Where(x => x.UserName == _selectedUser).Select(x => x.Id).FirstOrDefault();
            await ServiceDeskApi.SendDataToServerAsync(NewTask, ServiceDeskApi.ApiEnum.EditTask);
            await Navigation.PopAsync();
        }

        #region UPDATE DATA

        public async Task UpdateUsers()
        {
            Users.Clear();
            var users = await ServiceDeskApi.GetAllUsersAsync(new { User_id = default(string), Search = default(string) }, ServiceDeskApi.ApiEnum.GetUsersList);
            Users = new ObservableCollection<UserModel>(users);
            UsersNames = new ObservableCollection<string>(users.Select(x => x.UserName));
            SelectedRecipent = TaskEdit.Recipient_name ?? default(string); 
        }

        /// <summary>
        /// Обновляет типы заявок
        /// </summary>
        public async Task UpdateTypes()
        {

            Title = TaskEdit.Title;
            Text = TaskEdit.Text;
            var typesTasks = await ServiceDeskApi.GetDataServisDeskManagmentAsync<ServiceDesk_TypeListView>(ServiceDeskApi.ApiEnum.GetTypes);
            Types.Clear();
            foreach (var t in typesTasks)
            {
                Types.Add(t);
            }
            SelectedTypes = TaskEdit.Type_name != null ? typesTasks.Where(x => x.Type_name == TaskEdit.Type_name).FirstOrDefault() : default(ServiceDesk_TypeListView);
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
            SelectedFactory = factorys.Where(x => x.Factory_name == TaskEdit.Factory_name).FirstOrDefault();
        }

        /// <summary>
        /// Обновляет линии
        /// </summary>
        private void UpdatePlants(params int[] args)
        {
            Plants.Clear();

            IEnumerable<Product_PlantListView> plants = ServiceDeskApi.GetProductUnit<Product_PlantListView>(ServiceDeskApi.ApiEnum.GetProductPlantList).Where(x => x.Factory_id == args[0]);

            foreach (var p in plants)
            {
                Plants.Add(p);
            }

            SelectedPlant = TaskEdit.Plant_name != null ? plants.Where(x => x.Plant_name == TaskEdit.Plant_name).FirstOrDefault() : default(Product_PlantListView);
        }

        public async Task UpdatePlantsAsync()
        {
            Plants.Clear();

            IEnumerable<Product_PlantListView> plants = await ServiceDeskApi.GetProductUnitAsync<Product_PlantListView>(ServiceDeskApi.ApiEnum.GetProductPlantList);

            foreach (var p in plants)
            {
                Plants.Add(p);
            }

            SelectedPlant = TaskEdit.Plant_name != null ? plants.Where(x => x.Plant_name == TaskEdit.Plant_name).FirstOrDefault() : default(Product_PlantListView);
        }

        /// <summary>
        /// Обновляет юниты, args1 - factory id, args2 - plantid
        /// </summary>
        /// <param name="args"></param>
        private void UpdateUnits(params int[] args)
        {
            Units.Clear();
            IEnumerable<Product_UnitListView> units = ServiceDeskApi.GetProductUnit<Product_UnitListView>(ServiceDeskApi.ApiEnum.GetProductUnitList).Where(x => x.Plant_id == args[0]);
            
            foreach (var u in units)
            {
                Units.Add(u);
            }

            SelectedUnit = TaskEdit.Unit_name != null ? units.Where(x => x.Unit_name == TaskEdit.Unit_name).FirstOrDefault() : default(Product_UnitListView);
            
        }

        public async Task UpdateUnitsAsync()
        {
            Units.Clear();
            IEnumerable<Product_UnitListView> units = await ServiceDeskApi.GetProductUnitAsync<Product_UnitListView>(ServiceDeskApi.ApiEnum.GetProductUnitList);
            
            foreach (var u in units)
            {
                Units.Add(u);
            }

            SelectedUnit = TaskEdit.Unit_name != null ? units.Where(x => x.Unit_name == TaskEdit.Unit_name).FirstOrDefault() : default(Product_UnitListView);

        }
        #endregion
    }
}
