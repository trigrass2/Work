using System.ComponentModel;

namespace ServiceDesk.Models
{
    public class ServiceDesk_TypeListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID типа заявки
        /// </summary>
        public int Type_id { get; set; }

        /// <summary>
        /// Название типа заявки
        /// </summary>
        public string Type_name { get; set; }
    }
}
