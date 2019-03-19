using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Модель создания заявки
    /// </summary>
    public class CreateTaskModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID типа заявки
        /// </summary>
        public int? Type_id { get; set; }
        
        /// <summary>
        /// Заголовок заявки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст заявки
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Прикрепленные файлы
        /// </summary>
        public ObservableCollection<AttachmentFileModel> Attachments { get; set; }

        /// <summary>
        /// ID Завода
        /// </summary>
        public int? Factory_id { get; set; }

        /// <summary>
        /// ID Линии завода
        /// </summary>
        public int? Plant_id { get; set; }

        /// <summary>
        /// ID Производственной единицы 
        /// </summary>
        public int? Unit_id { get; set; }

        /// <summary>
        /// ID Производственной единицы
        /// </summary>
        public string Recipient_id { get; set; }
    }

    /// <summary>
    /// Модель прикрепленного файла
    /// </summary>
    public class AttachmentFileModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Attachment_name { get; set; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[] Attachment_bytes { get; set; }
    }
}
