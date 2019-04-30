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
using System.IO;

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
        public bool IsVisibleStatusButton { get; set; } = true;
        public bool IsEnableStatusButton { get; set; } = true;
        public bool IsVisibleFactory { get; set; } = true;
        public bool IsEdit { get; set; } = false;

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
                var model = new { ServiceDesk_TaskListView.Task_id, _selectedAttachment.Attachment_num };
                var files = GetTaskAttachments<ServiceDesk_TaskAttachmentListView>(model, ApiEnum.GetTaskAttachments);
                
                DownloadFiles(files.ElementAt(0).Attachment_name, files.ElementAt(0).Attachment_bytes);
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

            if (ServiceDesk_TaskListView.Status_id == 1) TextButton = "Начать";
            if (ServiceDesk_TaskListView.Status_id == 2) TextButton = "Завершить";
            if (ServiceDesk_TaskListView.Status_id == 50) TextButton = "Возобновить";
            if (ServiceDesk_TaskListView.Status_id > 50) TextButton = "Закрыто";

            UpdateAttachments(ServiceDesk_TaskListView.Task_id);
            UpdateComments();
            AddNewCommentCommand = new Command(AddComment);
            AddNewAttachmentCommand = new Command(AddNewAttachment);
            CallCommand = new Command(Call);
            GoToEdit = new Command(GoEdit);
            EditStatusCommand = new Command(EditStatus);
        }

        public void UpdateContext()
        {
            try
            {
                var d = ServiceDeskApi.GetData<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
                ServiceDesk_TaskListView = d.Where(x => x.Task_id == ServiceDesk_TaskListView.Task_id).FirstOrDefault();

                Log.WriteMessage("Обновление данных");
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при обновлении данных : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Скачивает файл на телефон
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataArray"></param>
        public async void DownloadFiles(string fileName, byte[] dataArray)
        {           
             
            var javafile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            var path = Path.Combine(javafile.AbsolutePath, fileName);

            try
            {
                if (await App.Current.MainPage.DisplayAlert("Сообщение", "Сохранить файл?", "Да", "Нет") == true)
                {

                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        Log.WriteMessage("Сохраняю файл...");

                        for (int i = 0; i < dataArray.Length; i++)
                        {
                            fileStream.WriteByte(dataArray[i]);
                        }

                        fileStream.Seek(0, SeekOrigin.Begin);

                        for (int i = 0; i < fileStream.Length; i++)
                        {
                            if (dataArray[i] != fileStream.ReadByte())
                            {
                                Log.WriteMessage("Error writing data");
                                await App.Current.MainPage.DisplayAlert("Error", "Error writing data.", "Ok");
                                return;
                            }
                        }
                        await App.Current.MainPage.DisplayAlert("Message", "Сохранено в Download", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка сохранения : {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Message", "Ошибка при попытке сохранения!", "Ok");
            }
            
        }

        /// <summary>
        /// Обновляет статусы
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStatuses()
        {
            Log.WriteMessage("Обновление списка статусов...");
            Statuses.Clear();
            try
            {
                var statuses = await GetDataServisDeskManagmentAsync<ServiceDesk_StatusListView>(ApiEnum.GetStatuses);
                foreach (var s in statuses)
                {
                    Statuses.Add(s);
                }

                Log.WriteMessage("Статусы обновлены.");

                SelectedStatus = statuses.Where(x => x.Status_id == ServiceDesk_TaskListView.Status_id).FirstOrDefault();
                if (SelectedStatus.Status_id > 50)
                {
                    IsVisibleStatusButton = false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка обновления статусов : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Отправляет на редактирование
        /// </summary>
        public async void GoEdit()
        {
            Log.WriteMessage("Переход на страницу редактирования");
            await Navigation.PushAsync(new EditTaskView(ServiceDesk_TaskListView));
        }

        /// <summary>
        /// Звонит
        /// </summary>
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
                    Log.WriteMessage($"Вызываю {ServiceDesk_TaskListView.Initiator_phone}");
                    Device.OpenUri(new Uri($"tel:{ServiceDesk_TaskListView.Initiator_phone}"));
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при попытке позвонить : {ex.Message}");
            }            
        }
        
        /// <summary>
        /// Обновляет список вложений
        /// </summary>
        /// <param name="taskId"></param>
        private async void UpdateAttachments(int taskId)
        {
            Attachments.Clear();
            AttachmentsNames = string.Empty;
            try
            {
                var attachments = await GetDataFromTask<ServiceDesk_TaskAttachmentInfoListView>(ApiEnum.GetTaskAttachmentsInfo, taskId);
                foreach (var a in attachments)
                {
                    Attachments.Add(a);
                }
                if (Attachments.Count > 0)
                {
                    IsVisibleAttach = true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при обновлении списка вложений : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Добавляет вложение
        /// </summary>
        public async void AddNewAttachment()
        {           
            file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                try
                {
                    var TaskAttachment = new { ServiceDesk_TaskListView.Task_id, Attachment_name = file.FileName, Attachment_bytes = file.DataArray };
                    SendDataToServer(TaskAttachment, ApiEnum.AddTaskAttachment);
                    UpdateAttachments(ServiceDesk_TaskListView.Task_id);

                    Log.WriteMessage($"Добавил вложение");
                }
                catch (Exception ex)
                {
                    Log.WriteMessage($"Ошибка при добавлении вложения : {ex.Message}");
                }                              
            }
        }

        /// <summary>
        /// Обновляет список комментариев
        /// </summary>
        private async void UpdateComments()
        {
            Comments.Clear();
            try
            {
                var comments = await GetDataFromTask<ServiceDesk_TaskCommentListView>(ApiEnum.GetTaskComments, ServiceDesk_TaskListView.Task_id);
                foreach (var c in comments)
                {
                    Comments.Add(c);
                }
                Log.WriteMessage("Обновил список комментариев");
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при обновлении комментариев : {ex.Message}");
            }
            
        }

        public string TextButton { get; set; }

        public void EditStatus(object statusName)
        {
            string st = statusName as string;
            var status = Statuses.Where(x => x.Status_name == st).FirstOrDefault();
            int id = status.Status_id;

            try
            {
                EditTaskModel NewTask = new EditTaskModel
                {
                    Task_id = ServiceDesk_TaskListView?.Task_id,
                    Type_id = ServiceDesk_TaskListView?.Type_id ?? null,
                    Factory_id = ServiceDesk_TaskListView?.Factory_id ?? null,
                    Plant_id = ServiceDesk_TaskListView?.Plant_id ?? null,
                    Unit_id = ServiceDesk_TaskListView?.Unit_id ?? null,
                    Recipient_id = ServiceDesk_TaskListView.Recipient_name,
                    Title = ServiceDesk_TaskListView.Title,
                    Text = ServiceDesk_TaskListView.Text
                };

                Log.WriteMessage($"Изменение статуса..");

                switch (id)
                {
                    case 1:
                        {
                            SelectedStatus = Statuses.Where(x => x.Status_id == 2).FirstOrDefault();
                            NewTask.Status_id = 2;
                            SendDataToServer(NewTask, ApiEnum.EditTask);
                            TextButton = "Завершить";
                            break;
                        }
                    case 2:
                        {
                            SelectedStatus = Statuses.Where(x => x.Status_id == 50).FirstOrDefault();
                            NewTask.Status_id = 50;
                            SendDataToServer(NewTask, ApiEnum.EditTask);
                            TextButton = "Возобновить";
                            break;
                        }
                    case 50:
                        {
                            SelectedStatus = Statuses.Where(x => x.Status_id == 2).FirstOrDefault();
                            NewTask.Status_id = 2;
                            SendDataToServer(NewTask, ApiEnum.EditTask);
                            TextButton = "Завершить";
                            break;
                        }
                }

                Log.WriteMessage($"Статус изменен");
                ServiceDesk_TaskListView = GetData<ServiceDesk_TaskListView>(ApiEnum.GetTasks).Where(x => x.Task_id == NewTask.Task_id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при изменении статуса : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Добовляет комментарий
        /// </summary>
        /// <param name="message"></param>
        private void AddComment(object message)
        {
            try
            {
                string comment = message as string;
                if (comment != "")
                {
                    SendDataToServer(
                    new
                    {
                        ServiceDesk_TaskListView.Task_id,
                        Text = comment
                    },
                    ApiEnum.AddTaskComment
                );
                    Log.WriteMessage("Добавил комментарий");
                    UpdateComments();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage($"Ошибка при добавлении комментария : {ex.Message}");
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
        
    }
}
