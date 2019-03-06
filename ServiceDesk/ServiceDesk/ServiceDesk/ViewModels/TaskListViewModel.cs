using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public TaskListViewModel()
        {
            CreateTaskCommand = new Command(GoInCreatePage);
            Tasks = new ObservableCollection<ServiceDesk_TaskListView>();            
            UpdateTasks();            
        }

        private void TaskListViewModel_Popped(object sender, NavigationEventArgs e)
        {
            UpdateTasks();
        }

        public async void GoInCreatePage()
        {           
            await Navigation.PushAsync(new SendTaskPage());
        }

        private async void UpdateTasks()
        {
            Tasks.Clear();
            var items = await ServiceDeskApi.GetDataFromTask<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
            foreach (var i in items)
            {
                Tasks.Add(i);
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
