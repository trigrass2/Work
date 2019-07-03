using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    public class FilterPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public ICommand OnFilterCommand { get; set; }

        public ServiceDesk_TaskListView Filter { get; set; }

        public ObservableCollection<Product_FactoryListView> Factorys { get; set; }
        public ObservableCollection<Product_PlantListView> Plants { get; set; }
        public ObservableCollection<Product_UnitListView> Units { get; set; }
        public ObservableCollection<ServiceDesk_TypeListView> Types { get; set; }
        public ObservableCollection<ServiceDesk_StatusListView> Statuses { get; set; }
        public ObservableCollection<ServiceDesk_TaskListView> Tasks { get; set; }

        private ServiceDesk_StatusListView _selectedStatus;
        public ServiceDesk_StatusListView SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
                if(_selectedStatus != null)
                {
                    Filter.Status_id = _selectedStatus.Status_id;
                    Filter.Status_name = _selectedStatus.Status_name;
                }
                
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
                if (_selectedPlant.Plant_id == 0)
                {
                    _selectedPlant = null;
                }
                Filter.Plant_id = _selectedPlant?.Plant_id;
                Filter.Plant_name = _selectedPlant?.Plant_name;
                
                UpdateUnits(SelectedFactory, SelectedPlant);
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
                if (_selectedUnit.Unit_id == 0)
                {
                    _selectedUnit = null;
                }
                Filter.Unit_id = _selectedUnit?.Unit_id;
                Filter.Unit_name = _selectedUnit?.Unit_name;
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
                if(_selectedFactory.Factory_id == 0)
                {
                    _selectedFactory = null;
                }
                Filter.Factory_id = _selectedFactory?.Factory_id;
                Filter.Factory_name = _selectedFactory?.Factory_name;
                UpdatePlants(SelectedFactory);
            }
        }
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
                if (_selectedType.Type_id == 0)
                {
                    _selectedType = null;
                }
                Filter.Type_id = _selectedType?.Type_id;
                Filter.Type_name = _selectedType?.Type_name;
            }
        }

        public FilterPageViewModel(ServiceDesk_TaskListView filter)
        {
            Filter = filter;
            Factorys = new ObservableCollection<Product_FactoryListView>();
            Plants = new ObservableCollection<Product_PlantListView>();
            Units = new ObservableCollection<Product_UnitListView>();
            Types = new ObservableCollection<ServiceDesk_TypeListView>();
            Statuses = new ObservableCollection<ServiceDesk_StatusListView>();
            Tasks = new ObservableCollection<ServiceDesk_TaskListView>();
            
            UpdateStatuses();
            UpdateTypes();
            UpdateFactorys();
            UpdatePlants(SelectedFactory);
            UpdateUnits(SelectedFactory, SelectedPlant);

            if(Filter != null)
            {
                SelectedStatus = Statuses.Where(x => x.Status_id == Filter?.Status_id).FirstOrDefault();
                SelectedFactory = Factorys.Where(x => x.Factory_id == Filter?.Factory_id).FirstOrDefault();
                SelectedPlant = Plants.Where(x => x.Plant_id == Filter?.Plant_id).FirstOrDefault();
                SelectedUnit = Units.Where(x => x.Unit_id == Filter?.Unit_id).FirstOrDefault();
                SelectedTypes = Types.Where(x => x.Type_id == Filter?.Type_id).FirstOrDefault();
            }           

            OnFilterCommand = new Command(OnFilter);
        }

        private void OnFilter()
        {           

            try
            {
                NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                var homePage = navStack[navPage.Navigation.NavigationStack.Count - 1] as MenuPage;

                if (homePage != null)
                {
                    homePage.ViewModel.Filter = Filter;
                    homePage.ViewModel.UpdateTasks(Filter.Status_id);
                }
                Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при выполнении UpdateContext() : {ex.Message}");
            }
        }

        #region UPDATE DATA

        /// <summary>
        /// Обновляет типы заявок
        /// </summary>
        public void UpdateStatuses()
        {
            try
            {
                Statuses.Clear();
                var statuses = ServiceDeskApi.GetDataServisDeskManagment<ServiceDesk_StatusListView>(ServiceDeskApi.ApiEnum.GetStatuses);
                Statuses.Add(new ServiceDesk_StatusListView { Status_name = "Все статусы" });

                foreach (var s in statuses)
                {
                    Statuses.Add(s);
                }
                
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при обновлении списка статусов : {ex.Message}");
            }

        }

        public void UpdateTypes()
        {            
            var typesTasks = ServiceDeskApi.GetDataServisDeskManagment<ServiceDesk_TypeListView>(ServiceDeskApi.ApiEnum.GetTypes);            

            Types.Clear();
            Types.Add(new ServiceDesk_TypeListView { Type_name = "Все катоегории" });

            foreach (var t in typesTasks)
            {
                Types.Add(t);
            }            
        }

        /// <summary>
        /// Обновляет список заводов
        /// </summary>
        public void UpdateFactorys()
        {            
            Factorys.Clear();

            var factorys = ServiceDeskApi.GetProductUnit<Product_FactoryListView>(ServiceDeskApi.ApiEnum.GetProductFactoryList);
            Factorys.Add(new Product_FactoryListView { Factory_name = "Все заводы" });

            foreach (var f in factorys)
            {
                Factorys.Add(f);
            }

            
        }

        /// <summary>
        /// Обновляет линии
        /// </summary>
        private void UpdatePlants(Product_FactoryListView selectedFactory)
        {
            Plants.Clear();
            IEnumerable<Product_PlantListView> plants;

            plants = ServiceDeskApi.GetProductUnit<Product_PlantListView>(ServiceDeskApi.ApiEnum.GetProductPlantList, selectedFactory?.Factory_id);
            Plants.Add(new Product_PlantListView { Plant_name = "Все линии" });

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
        private void UpdateUnits(Product_FactoryListView selectedFactory, Product_PlantListView selectedPlant)
        {
            Units.Clear();
            
            var units = ServiceDeskApi.GetProductUnit<Product_UnitListView>(ServiceDeskApi.ApiEnum.GetProductUnitList, selectedPlant?.Plant_id, selectedFactory?.Factory_id);
            Units.Add(new Product_UnitListView { Unit_name = "Все блоки" });

            foreach (var u in units)
            {
                Units.Add(u);
            }            
        }
        #endregion

    }
}
