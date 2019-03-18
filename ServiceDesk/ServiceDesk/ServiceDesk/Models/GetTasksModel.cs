using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Модель заявки для отправки
    /// </summary>
    public class GetTasksModel : INotifyPropertyChanged
    {
        /// <summary>
        /// ID статуса заявки
        /// </summary>
        public int Status_id { get; set; }

        /// <summary>
        /// ID типа заявки
        /// </summary>
        public int Type_id { get; set; }

        /// <summary>
        /// ID завода
        /// </summary>
        public int Factory_id { get; set; }

        /// <summary>
        /// ID Линии производства
        /// </summary>
        public int Plant_id { get; set; }

        /// <summary>
        /// ID производственной линии
        /// </summary>
        public int Unit_id { get; set; }

        /// <summary>
        /// ID производственной линии
        /// </summary>
        public string Recipient_id { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
