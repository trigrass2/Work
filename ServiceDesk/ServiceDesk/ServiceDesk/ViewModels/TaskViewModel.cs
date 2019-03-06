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
        public string AttachmentsNames { get; set; }
                
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand AddNewAttachmentCommand { get; set; }

        FileData file = new FileData();

        public TaskViewModel(ServiceDesk_TaskListView serviceDesk_Task)
        {            
            Attachments = new ObservableCollection<ServiceDesk_TaskAttachmentInfoListView>();
            Comments = new ObservableCollection<ServiceDesk_TaskCommentListView>();
            ServiceDesk_TaskListView = serviceDesk_Task;
            UpdateAttachments(ServiceDesk_TaskListView.Task_id);
            UpdateComments();
            AddNewCommentCommand = new Command(AddComment);
            AddNewAttachmentCommand = new Command(AddNewAttachment);
        }
       
        public void DownloadFiles()
        {
            var model = new { Task_id = 1, Attachment_num = 1 };
            var files = GetTaskAttachments<ServiceDesk_TaskAttachmentListView>(model, ApiEnum.GetTaskAttachments);
        }

        private async void UpdateAttachments(int taskId)
        {
            Attachments.Clear();
            AttachmentsNames = string.Empty;
            var attachments = await GetDataFromTask<ServiceDesk_TaskAttachmentInfoListView>(ApiEnum.GetTaskAttachmentsInfo, taskId);
            foreach(var a in attachments)
            {
                Attachments.Add(a);
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
            foreach(var c in comments)
            {
                Comments.Add(c);
            }
        }

        /// <summary>
        /// Добовляет комментарий
        /// </summary>
        /// <param name="message"></param>
        private void AddComment(object message)
        {
            SendDataToServer(
                new AddTaskCommentModel
                {
                    Task_id = ServiceDesk_TaskListView.Task_id,
                    Text = message as string
                },
                ApiEnum.AddTaskComment
            );
            UpdateComments();
        }

        public TaskListViewModel TaskListViewModel
        {
            get
            {
                return _taskViewModel;
            }
            set
            {
                if(_taskViewModel != value)
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
                return  (!string.IsNullOrEmpty(Type_name.Trim())) ||
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
