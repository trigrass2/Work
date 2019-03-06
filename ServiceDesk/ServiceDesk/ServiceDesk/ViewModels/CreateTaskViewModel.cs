using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    /// <summary>
    /// Логика создания запроса
    /// </summary>
    public class CreateTaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CreateTaskModel NewTask { get; set; }

        /// <summary>
        /// Отправляет новую заявку
        /// </summary>
        public ICommand SendTaskCommand { get; set; }
        public ICommand GetFileCommand { get; set; }
        public INavigation Navigation { get; set; }
        FileData file = new FileData();

        public string Type_id
        {
            get
            {
                return NewTask.Type_id.ToString();
            }
            set
            {
                try
                {
                    NewTask.Type_id = int.Parse(value);
                }
                catch
                {
                    NewTask.Type_id = null;   
                }
            }
        }

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

        public string Factory_id
        {
            get
            {
                return NewTask.Factory_id.ToString();
            }
            set
            {
                try
                {
                    NewTask.Factory_id = int.Parse(value);
                }
                catch
                {
                    NewTask.Factory_id = null;
                }
            }
        }

        public ObservableCollection<AttachmentFileModel> Attachments
        {
            get
            {
                return NewTask.Attachments;
            }
            set
            {
                NewTask.Attachments = value;
            }

        }

        public string Plant_id
        {
            get
            {
                return NewTask.Plant_id.ToString();
            }
            set
            {
                try
                {
                    NewTask.Plant_id = int.Parse(value);
                }
                catch
                {
                    NewTask.Plant_id = null;
                }
            }
        }

        public string Unit_id
        {
            get
            {
                return NewTask.Unit_id.ToString();
            }
            set
            {
                try
                {
                    NewTask.Unit_id = int.Parse(value);
                }
                catch
                {
                    NewTask.Unit_id = null;
                }
            }
        }

        public string Recipient_id
        {
            get
            {
                return NewTask.Recipient_id;
            }
            set
            {
                NewTask.Recipient_id = value;
            }
        }
        #endregion

        public CreateTaskViewModel()
        {
            
            NewTask = new CreateTaskModel() { Attachments = new ObservableCollection<AttachmentFileModel>() };
            SendTaskCommand = new Command(SendTask);
            GetFileCommand = new Command(GetFile);           
        }

        /// <summary>
        /// отправляет данные
        /// </summary>
        public async void SendTask()
        {            
            ServiceDeskApi.SendDataToServer(NewTask, ServiceDeskApi.ApiEnum.CreateTask);
            await Navigation.PopAsync();
        }

        /// <summary>
        /// полчает данные выбранного файла
        /// </summary>
        public async void GetFile()
        {
            file = await CrossFilePicker.Current.PickFile();            
            
            if (file != null)
            {                
                NewTask.Attachments.Add(new AttachmentFileModel {Attachment_name = file.FileName, Attachment_bytes = file.DataArray });
            }
        }
    }
}
