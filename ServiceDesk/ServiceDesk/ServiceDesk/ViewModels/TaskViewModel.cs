using ServiceDesk.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static ServiceDesk.PikApi.ServiceDeskApi;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using System.Threading.Tasks;
using System.Linq;

namespace ServiceDesk.ViewModels
{
    /// <summary>
    /// Взаимодействие с заявкой
    /// </summary>
    public class TaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ServiceDesk_TaskListView ServiceDesk_TaskListView { get; set; }
        public ObservableCollection<ServiceDesk_TaskCommentListView> ServiceDesk_TaskCommentListViews { get; set; }
        private TaskListViewModel _taskViewModel;
        public ObservableCollection<ServiceDesk_TaskCommentListView> Comments { get; set; }
        public ObservableCollection<ServiceDesk_TaskAttachmentInfoListView> Attachments { get; set; }
        public ObservableCollection<ServiceDesk_StatusListView> Statuses { get; set; }
        public string AttachmentsNames { get; set; }

        public INavigation Navigation { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand AddNewAttachmentCommand { get; set; }
        public ICommand CallCommand { get; set; }
        public ICommand GoToEdit { get; set; }
        public ICommand EditStatusCommand { get; set; }
        public ICommand DownLoadAttachmentsCommand { get; set; }

        public bool IsVisiblePlant { get; set; } = true;
        public bool IsVisibleUnit { get; set; } = true;
        public bool IsVisibleAttach { get; set; } = true;

        private ServiceDesk_TaskAttachmentInfoListView _selectedAttachment;
        public ServiceDesk_TaskAttachmentInfoListView SelectedAttachment
        {
            get
            {
                return _selectedAttachment;
            }
            set
            {
                _selectedAttachment = value;
                //var model = new { ServiceDesk_TaskListView.Task_id, _selectedAttachment.Attachment_num };
                //var files = GetTaskAttachments<ServiceDesk_TaskAttachmentListView>(model, ApiEnum.GetTaskAttachments);
            }
        }
        public ServiceDesk_StatusListView SelectedStatus { get; set; }
        FileData file = new FileData();

        public TaskViewModel(ServiceDesk_TaskListView serviceDesk_Task)
        {
            Statuses = new ObservableCollection<ServiceDesk_StatusListView>();
            Attachments = new ObservableCollection<ServiceDesk_TaskAttachmentInfoListView>();
            Comments = new ObservableCollection<ServiceDesk_TaskCommentListView>();
            ServiceDesk_TaskListView = serviceDesk_Task;

            if (ServiceDesk_TaskListView.Status_name == "Открыта") TextButton = "Начать";
            if (ServiceDesk_TaskListView.Status_name == "В работе") TextButton = "Завершить";
            if (ServiceDesk_TaskListView.Status_name == "Закрыта") TextButton = "Начать";

            UpdateAttachments(ServiceDesk_TaskListView.Task_id);
            UpdateComments();
            AddNewCommentCommand = new Command(AddComment);
            AddNewAttachmentCommand = new Command(AddNewAttachment);
            CallCommand = new Command(Call);
            GoToEdit = new Command(GoEdit);
            EditStatusCommand = new Command(EditStatus);
        }

        //public void DownloadFiles(byte[] byteOfFile)
        //{

        //}

        public async Task UpdateStatuses()
        {
            Statuses.Clear();
            var statuses = await GetDataServisDeskManagmentAsync<ServiceDesk_StatusListView>(ApiEnum.GetStatuses);
            foreach(var s in statuses)
            {
                Statuses.Add(s);
            }
            
            SelectedStatus = statuses.Where(x => x.Status_id == ServiceDesk_TaskListView.Status_id).FirstOrDefault();
        }
        public async void GoEdit()
        {
            await Navigation.PushAsync(new EditTaskView(ServiceDesk_TaskListView));
        }

        public async void Call()
        {
            try
            {
                if (ServiceDesk_TaskListView.Initiator_phone == "")
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Specify the number to start the call.", "OK");
                }
                else
                {
                   await DependencyService.Get<IPhoneCall>()?.MakeQuickCall(ServiceDesk_TaskListView.Initiator_phone);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        private async void UpdateAttachments(int taskId)
        {
            Attachments.Clear();
            AttachmentsNames = string.Empty;
            var attachments = await GetDataFromTask<ServiceDesk_TaskAttachmentInfoListView>(ApiEnum.GetTaskAttachmentsInfo, taskId);
            foreach (var a in attachments)
            {
                Attachments.Add(a);
            }
            if(Attachments.Count > 0)
            {
                IsVisibleAttach = true;
            }
        }

        public async void AddNewAttachment()
        {           
            file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                var TaskAttachment = new { ServiceDesk_TaskListView.Task_id, Attachment_name = file.FileName, Attachment_bytes = file.DataArray };
                SendDataToServer(TaskAttachment, ApiEnum.AddTaskAttachment);
                UpdateAttachments(ServiceDesk_TaskListView.Task_id);
                
            }
        }

        /// <summary>
        /// Обновляет список комментариев
        /// </summary>
        private async void UpdateComments()
        {
            Comments.Clear();
            var comments = await GetDataFromTask<ServiceDesk_TaskCommentListView>(ApiEnum.GetTaskComments, ServiceDesk_TaskListView.Task_id);
            foreach (var c in comments)
            {
                Comments.Add(c);
            }
            
        }

        public string TextButton { get; set; }

        public void EditStatus(object statusName)
        {
            string st = statusName as string;
            var status = Statuses.Where(x => x.Status_name == st).FirstOrDefault();
            int id = status.Status_id;
            EditTaskModel NewTask = new EditTaskModel();
            NewTask.Task_id = ServiceDesk_TaskListView?.Task_id;
            NewTask.Type_id = ServiceDesk_TaskListView?.Type_id ?? null;
            NewTask.Factory_id = ServiceDesk_TaskListView?.Factory_id ?? null;
            NewTask.Plant_id = ServiceDesk_TaskListView?.Plant_id ?? null;
            NewTask.Unit_id = ServiceDesk_TaskListView?.Unit_id ?? null;
            NewTask.Recipient_id = ServiceDesk_TaskListView.Recipient_name;
            NewTask.Title = ServiceDesk_TaskListView.Title;
            NewTask.Text = ServiceDesk_TaskListView.Text;

            switch (id)
            {
                case 1:
                    {
                        SelectedStatus = Statuses.Where(x => x.Status_id == 2).FirstOrDefault(); ;
                        NewTask.Status_id = 2;
                        SendDataToServer(NewTask, ApiEnum.EditTask);
                        TextButton = "Завершить";
                        break;
                    }
                case 2:
                    {
                        SelectedStatus = Statuses.Where(x => x.Status_id == 50).FirstOrDefault(); ;
                        NewTask.Status_id = 50;
                        SendDataToServer(NewTask, ApiEnum.EditTask);
                        TextButton = "Начать";
                        break;
                    }                     
            }
            ServiceDesk_TaskListView = GetData<ServiceDesk_TaskListView>(ApiEnum.GetTasks).Where(x => x.Task_id == NewTask.Task_id).FirstOrDefault();
        }

        /// <summary>
        /// Добовляет комментарий
        /// </summary>
        /// <param name="message"></param>
        private void AddComment(object message)
        {
            
            string comment = message as string;
            if(comment != "Добавить комментарий" || comment != "")
            {
                SendDataToServer(
                new AddTaskCommentModel
                {
                    Task_id = ServiceDesk_TaskListView.Task_id,
                    Text = comment
                },
                ApiEnum.AddTaskComment
            );
                UpdateComments();
            }
            
        }

        public TaskListViewModel TaskListViewModel
        {
            get
            {
                return _taskViewModel;
            }
            set
            {
                if (_taskViewModel != value)
                {
                    _taskViewModel = value;
                }
            }
        }

        #region поля заявки

        public int Task_id
        {
            get
            {
                return ServiceDesk_TaskListView.Task_id;
            }
            set
            {
                ServiceDesk_TaskListView.Task_id = value;
            }
        }

        public string Type_name
        {
            get
            {
                return ServiceDesk_TaskListView.Type_name;
            }
            set
            {
                ServiceDesk_TaskListView.Type_name = value;
            }
        }

        public string Status_name
        {
            get
            {
                return ServiceDesk_TaskListView.Status_name;
            }
            set
            {
                ServiceDesk_TaskListView.Status_name = value;
            }
        }

        public DateTime Status_timestamp
        {
            get
            {
                return ServiceDesk_TaskListView.Status_timestamp;
            }
            set
            {
                ServiceDesk_TaskListView.Status_timestamp = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return ServiceDesk_TaskListView.Timestamp;
            }
            set
            {
                ServiceDesk_TaskListView.Timestamp = value;
            }
        }

        public string Title
        {
            get
            {
                return ServiceDesk_TaskListView.Title;
            }
            set
            {
                ServiceDesk_TaskListView.Title = value;
            }
        }

        public string Text
        {
            get
            {
                return ServiceDesk_TaskListView.Text;
            }
            set
            {
                ServiceDesk_TaskListView.Text = value;
            }
        }

        public string Initiator_name
        {
            get
            {
                return ServiceDesk_TaskListView.Initiator_name;
            }
            set
            {
                ServiceDesk_TaskListView.Initiator_name = value;
            }
        }

        public string Recipient_name
        {
            get
            {
                return ServiceDesk_TaskListView.Recipient_name;
            }
            set
            {
                ServiceDesk_TaskListView.Recipient_name = value;
            }
        }

        public string Factory_name
        {
            get
            {
                return ServiceDesk_TaskListView.Factory_name;
            }
            set
            {
                ServiceDesk_TaskListView.Factory_name = value;
            }
        }

        public string Plant_name
        {
            get
            {
                return ServiceDesk_TaskListView.Plant_name;
            }
            set
            {
                ServiceDesk_TaskListView.Plant_name = value;
            }
        }

        public string Unit_name
        {
            get
            {
                return ServiceDesk_TaskListView.Unit_name;
            }
            set
            {
                ServiceDesk_TaskListView.Unit_name = value;
            }
        }

        #endregion

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Type_name.Trim())) ||
                        (!string.IsNullOrEmpty(Status_name.Trim())) ||
                        (!string.IsNullOrEmpty(Title.Trim())) ||
                        (!string.IsNullOrEmpty(Text.Trim())) ||
                        (!string.IsNullOrEmpty(Initiator_name.Trim())) ||
                        (!string.IsNullOrEmpty(Recipient_name.Trim())) ||
                        (!string.IsNullOrEmpty(Plant_name.Trim())) ||
                        (!string.IsNullOrEmpty(Unit_name.Trim()));
            }
        }
    }
}
