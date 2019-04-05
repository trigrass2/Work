using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using Xamarin.Forms;
using Com.OneSignal;
using System.Collections.Generic;
using System;

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
        public ICommand OpenProfileCommand { get; set; }

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
        public bool IsBoosy { get; set; } = true;
        
        public TaskListViewModel()
        {            
            Tasks = new ObservableCollection<ServiceDesk_TaskListView>();
            Statuses = new ObservableCollection<ServiceDesk_StatusListView>();
            CreateTaskCommand = new Command(GoInCreatePage);
            OpenProfileCommand = new Command(OpenProfile);
        }
        
        /// <summary>
        /// переходит на страницу профиля
        /// </summary>
        public async void OpenProfile()
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        /// <summary>
        /// переходит на страницу создания заявки
        /// </summary>
        public async void GoInCreatePage()
        {
            await Navigation.PushAsync(new SendTaskPage());
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
            SelectedStatus = Statuses.Where(x => x.Status_id == 1).FirstOrDefault();
        }

        /// <summary>
        /// Обновляет заявки
        /// </summary>
        /// <returns></returns>
        public async Task UpdateTasksAsync(int statusId)
        {
            var items = await ServiceDeskApi.GetDataAsync<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
            if(statusId != 3740)
            {
                items = items.Where(x => x.Status_id == statusId);
            }
            Tasks.Clear();
            
            if(items != null)
            {
                foreach (var i in items)
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

        private bool IsInitialiseSubscribed { get; set; } = false;
        ApplicationUser _user;

        public async Task UpdateSubscribed()
        {
            IsInitialiseSubscribed = true;
            _user = await ServiceDeskApi.GetUserAsync<ApplicationUser>(ServiceDeskApi.ApiEnum.GetUserInfo);

            OneSignal.Current.GetTags(TagsReceived);
        }

        private void TagsReceived(Dictionary<string, object> tags)
        {
            if (tags == null)
            {
                tags = new Dictionary<string, object>();
            }
            MakeSublistAsync(tags);
        }

        private async void MakeSublistAsync(Dictionary<string, object> tags)
        {
            try
            {
                foreach(var t in tags)
                {                    
                    OneSignal.Current.DeleteTag(t.Key.ToString());
                }
                OneSignal.Current.SendTag(_user.Id, "User_id");
                return;                
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.ToString(), "OK");
                return;
            }
        }        
    }
}
