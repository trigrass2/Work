using System;
using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Модель вложения
    /// </summary>
    public class ServiceDesk_TaskAttachmentInfoListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID заявки
        /// </summary>
        public int Task_id { get; set; }

        /// <summary>
        /// Номер вложения
        /// </summary>
        public int Attachment_num { get; set; }

        /// <summary>
        /// Имя вложения
        /// </summary>
        public string Attachment_name { get; set; }

        /// <summary>
        /// Длина вложения
        /// </summary>
        public int Attachment_length { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ID отправителя
        /// </summary>
        public string User_id { get; set; }

        /// <summary>
        /// Имя отправителя
        /// </summary>
        public string User_name { get; set; }
    }

    /// <summary>
    /// Модель прикрепленного файла
    /// </summary>
    public class AttachmentFileModel : ServiceDesk_TaskAttachmentInfoListView
    {       
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[] Attachment_bytes { get; set; }
    }
}
