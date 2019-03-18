using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public INavigation Navigation { get; set; }
        public ICommand CreateTaskCommand { get; set; }
        public ICommand GoToSettings { get; set; }

        public bool IsBusy { get; set; }
        public bool IsLoaded
        {
            get { return !IsBusy; }
        }

        public TaskListViewModel()
        {
            
            Tasks = new ObservableCollection<ServiceDesk_TaskListView>();
            IsBusy = false;
            CreateTaskCommand = new Command(GoInCreatePage);
            GoToSettings = new Command(GoToSettingsPage);                                   
        }

        public async void GoToSettingsPage()
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        public async void GoInCreatePage()
        {
            await Navigation.PushAsync(new SendTaskPage());
        }

        public async Task UpdateTasks()
        {
            IsBusy = true;
            
            var items = await ServiceDeskApi.GetDataAsync<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
            Tasks.Clear();
            
            if(items != null)
            {
                foreach (var i in items)
                {
                    Tasks.Add(i);
                }
            }
            IsBusy = false;
            
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
