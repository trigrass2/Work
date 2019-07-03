
using System.ComponentModel;

namespace ServiceDesk.Models
{
    public class EditTaskModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID заявки
        /// </summary>
        public int? Task_id { get; set; }

        /// <summary>
        /// ID типа заявки
        /// </summary>
        public int? Type_id { get; set; }

        /// <summary>
        /// ID статуса заявки
        /// </summary>
        public int? Status_id { get; set; }      
        
        /// <summary>
        /// Заголовок заявки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст заявки
        /// </summary>
        public string Text { get; set; }

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
}
