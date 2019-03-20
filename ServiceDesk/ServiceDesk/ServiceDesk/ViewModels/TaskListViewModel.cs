using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    /// <summary>
    /// Взаимодействие со списком заявок
    /// </summary>
    public class TaskListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ServiceDesk_TaskListView> Tasks { get; set; }
        public ObservableCollection<ServiceDesk_StatusListView> Statuses { get; set; }

        public INavigation Navigation { get; set; }
        public ICommand CreateTaskCommand { get; set; }
        public ICommand GoToSettings { get; set; }

        private ServiceDesk_StatusListView _allTasksStatus = new ServiceDesk_StatusListView { Status_id = 3740, Status_name = "Все заявки" };
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
                UpdateTasks(_selectedStatus.Status_id);
            }
        }

        public TaskListViewModel()
        {            
            Tasks = new ObservableCollection<ServiceDesk_TaskListView>();
            Statuses = new ObservableCollection<ServiceDesk_StatusListView>();
            CreateTaskCommand = new Command(GoInCreatePage);
            GoToSettings = new Command(GoToSettingsPage);
        }

        /// <summary>
        /// обновляет список заявок
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStatuses()
        {
            Statuses.Clear();
            var statuses = await ServiceDeskApi.GetDataServisDeskManagmentAsync<ServiceDesk_StatusListView>(ServiceDeskApi.ApiEnum.GetStatuses);
            foreach (var s in statuses)
            {
                Statuses.Add(s);
            }
            Statuses.Add(_allTasksStatus);
            SelectedStatus = _allTasksStatus;
        }
        /// <summary>
        /// Переходит на страницу настроек
        /// </summary>
        public async void GoToSettingsPage()
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        /// <summary>
        /// переходит на страницу создания заявки
        /// </summary>
        public async void GoInCreatePage()
        {
            await Navigation.PushAsync(new SendTaskPage());
        }
        
        /// <summary>
        /// Обновляет заявки
        /// </summary>
        /// <returns></returns>
        public async Task UpdateTasksAsync(int statusId)
        {            
            var items = await ServiceDeskApi.GetDataAsync<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
            var sortItems = items.OrderBy(x => x.Status_id).Select(x => x);
            if(statusId != 3740)
            {
                sortItems = sortItems.Where(x => x.Status_id == statusId);
            }
            Tasks.Clear();
            
            if(sortItems != null)
            {
                foreach (var i in sortItems)
                {                    
                    Tasks.Add(i);
                }                
            }
        }

        public void UpdateTasks(int statusId)
        {
            var items = ServiceDeskApi.GetData<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
            var sortItems = items.OrderBy(x => x.Status_id).Select(x => x);
            if (statusId != 3740)
            {
                sortItems = sortItems.Where(x => x.Status_id == statusId);
            }
            Tasks.Clear();

            if (sortItems != null)
            {
                foreach (var i in sortItems)
                {
                    Tasks.Add(i);
                }

            }
        }

        private ServiceDesk_TaskListView _selectedTask;
        public ServiceDesk_TaskListView SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                ServiceDesk_TaskListView tempTask = value;
                _selectedTask = null;
                Navigation.PushAsync(new SelectedTaskPage(new TaskViewModel(tempTask)));
            }
        }        
    }
}
