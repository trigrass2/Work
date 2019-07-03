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

        public ServiceDesk_TaskListView Filter { get; set; }

        private ObservableCollection<ServiceDesk_TaskListView> _tasks;
        public ObservableCollection<ServiceDesk_TaskListView> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
            }
        }
        public ObservableCollection<ServiceDesk_StatusListView> Statuses { get; set; }

        public INavigation Navigation { get; set; }
        public ICommand CreateTaskCommand { get; set; }
        public ICommand OpenProfileCommand { get; set; }
        public ICommand NextButtonCommand { get; set; }
        public ICommand BackButtonComand { get; set; }
        public ICommand OpenFilterCommand { get; set; }

        public int Page { get; set; }

        private bool _isEnableBackButton;
        public bool IsEnableBackButton
        {
            get
            {
                return _isEnableBackButton;
            }
            set
            {
                _isEnableBackButton = value;
            }
        }

        public Color StatusColor { get; set; } = Color.FromHex("#ffff");        

        public bool IsBoosy { get; set; } = true;
        
        public TaskListViewModel()
        {
            UpdateSubscribed();
            IsEnableNextButton = true;
            IsEnableBackButton = false;
            Filter = new ServiceDesk_TaskListView();
            Tasks = new ObservableCollection<ServiceDesk_TaskListView>();
            Statuses = new ObservableCollection<ServiceDesk_StatusListView>();
            CreateTaskCommand = new Command(GoInCreatePage);
            OpenProfileCommand = new Command(OpenProfile);
            NextButtonCommand = new Command(NextPages);
            BackButtonComand = new Command(BackPages);
            OpenFilterCommand = new Command(OpenFilter);

            UpdateTasks(Filter.Status_id);
        }

        public async void OpenFilter()
        {
            await Navigation.PushModalAsync(new FilterPage(Filter)); 
        }

        public void NextPages()
        {
            Page++;
            IsEnableBackButton = true;
            UpdateTasks(Filter.Status_id);
        }

        public void BackPages()
        {
            if (Page > 0)
            {
                IsEnableNextButton = true;
                Page--;
                UpdateTasks(Filter.Status_id);
            }
            else IsEnableBackButton = false;
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
        /// обновляет список статусов
        /// </summary>
        /// <returns></returns>
        //public async Task UpdateStatusesAsync()
        //{            
        //    try
        //    {
        //        Statuses.Clear();
        //        var statuses = await ServiceDeskApi.GetDataServisDeskManagmentAsync<ServiceDesk_StatusListView>(ServiceDeskApi.ApiEnum.GetStatuses);
        //        foreach (var s in statuses)
        //        {
        //            Statuses.Add(s);
        //        }

        //        //Statuses.Add(_allTasksStatus);
        //        //SelectedStatus = _allTasksStatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteMessage($"Ошибка при обновлении списка статусов : {ex.Message}");
        //    }            
        //}

        //public void UpdateStatuses()
        //{
        //    try
        //    {
        //        Statuses.Clear();
        //        var statuses = ServiceDeskApi.GetDataServisDeskManagment<ServiceDesk_StatusListView>(ServiceDeskApi.ApiEnum.GetStatuses);

        //        foreach (var s in statuses)
        //        {
        //            Statuses.Add(s);
        //        }

        //        //Statuses.Add(_allTasksStatus);
        //        //SelectedStatus = _allTasksStatus;//Statuses.Where(x => x.Status_id == 1).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteMessage($"Ошибка при обновлении списка статусов : {ex.Message}");
        //    }

        //}

        public bool _isEnableNextButton;
        /// <summary>
        /// Обновляет заявки
        /// </summary>
        /// <returns></returns>       
        public bool IsEnableNextButton
        {
            get
            {
                return _isEnableNextButton;
            }
            set
            {
                _isEnableNextButton = value;
            }
        }

        public void UpdateTasks(int statusId)
        {
            try
            {
                IEnumerable<ServiceDesk_TaskListView> items;
                Log.WriteMessage($"Обновление заявок...");

                if (statusId != 0)
                {
                    items = ServiceDeskApi.GetTasksPages(new { Filter.Status_id, Filter.Type_id, Filter.Factory_id, Filter.Plant_id, Filter.Unit_id, Page = Page, Amount = 10 });
                }
                else
                {
                    items = ServiceDeskApi.GetTasksPages(new { Filter.Type_id, Filter.Factory_id, Filter.Plant_id, Filter.Unit_id, Page = Page, Amount = 10 });
                }

                Tasks.Clear();

                if (items != null && items.Count() > 0 && Tasks != null)
                {
                    foreach (var i in items)
                    {
                        Tasks.Add(i);
                        switch (i.Status_id)
                        {
                            case 1: StatusColor = Color.FromHex("#ff8c00"); break;
                            case 51: StatusColor = Color.FromHex("#2e8b57"); break;
                            case 50: StatusColor = Color.FromHex("#4682b4"); break;
                            case 2: StatusColor = Color.FromHex("#ffff"); break;
                        }
                    }
                    if (Tasks.Count < 10)
                    {
                        IsEnableNextButton = false;
                    }
                    else IsEnableNextButton = true;
                }

                Log.WriteMessage($"Список заявок обновлен");
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при обновлении списка заявок : {ex.Message}");
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

        private void MakeSublistAsync(Dictionary<string, object> tags)
        {
            try
            {
                Log.WriteMessage($"Подписывание юзера");
                
                foreach (var t in tags)
                {                    
                    OneSignal.Current.DeleteTag(t.Key.ToString());
                }
                OneSignal.Current.SendTag(_user?.Id, "User_id");

                Log.WriteMessage($"Подписка обновлена");

                return;                
            }
            catch (Exception ex)
            {
                ServiceDeskApi.SendErrorToTelegram($"{ex.Message}");
                //OneSignal.Current.SendTag(_user?.Id, "User_id");
                //Log.WriteMessage($"Ошибка подписки : {e.Message}");
                return;
            }
        }        
    }
}
