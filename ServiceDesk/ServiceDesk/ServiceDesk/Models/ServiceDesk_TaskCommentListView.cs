using System;
using System.ComponentModel;

namespace ServiceDesk.Models
{    
    [Serializable]
    /// <summary>
    /// Модель комментария
    /// </summary>
    public class ServiceDesk_TaskCommentListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// ID заявки
        /// </summary>
        public int Task_id { get; set; }
        
        /// <summary>
        /// Номер комментария
        /// </summary>
        public int Comment_num { get; set; }
        
        /// <summary>
        /// ID комментатора
        /// </summary>
        public string Commentator_id { get; set; }
        
        /// <summary>
        /// Имя комментатора
        /// </summary>
        public string Commentator_name { get; set; }

        /// <summary>
        /// Время создания 
        /// </summary>
        public DateTime Timestamp { get; set; }
    
        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? Timestamp_edited { get; set; }        
    }
}
