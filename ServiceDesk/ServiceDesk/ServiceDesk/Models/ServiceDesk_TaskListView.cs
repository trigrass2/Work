using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ServiceDesk.Models
{
    
    /// <summary>
    /// Модель заявки для отображения
    /// </summary>
    public class ServiceDesk_TaskListView : INotifyPropertyChanged
    {        
        /// <summary>
        /// ID заявки
        /// </summary>
        public int Task_id { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        public string Type_name { get; set; }
        
        /// <summary>
        /// Статус заявки
        /// </summary>
        public string Status_name { get; set; }
        
        /// <summary>
        /// Время назначения статуса заявки
        /// </summary>
        public DateTime Status_timestamp { get; set; }
        
        /// <summary>
        /// Время создания заявки
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Заголовок заякви
        /// </summary>
        public  string Title { get; set; }
        
        /// <summary>
        /// Текст заявки
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Создатель заявки
        /// </summary>
        public string Initiator_name { get; set; }

        /// <summary>
        /// Номер инициатора
        /// </summary>
        public string Initiator_phone { get; set; }
        
        /// <summary>
        /// Получатель заявки
        /// </summary>
        public string Recipient_name { get; set; }
        
        /// <summary>
        /// Телефон исполнителя
        /// </summary>
        public string Recipient_phone { get; set; }

        /// <summary>
        /// Завод
        /// </summary>
        public string Factory_name { get; set; }
        
        /// <summary>
        /// Линия
        /// </summary>
        public string Plant_name { get; set; }
        
        /// <summary>
        /// Подразделение
        /// </summary>
        public string Unit_name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
