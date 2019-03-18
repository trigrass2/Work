using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Линия
    /// </summary>
    public class Product_PlantListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID линии
        /// </summary>
        public int Plant_id { get; set; }

        /// <summary>
        /// Название линии
        /// </summary>
        public string Plant_name { get; set; }

        /// <summary>
        /// ID завода
        /// </summary>
        public int Factory_id { get; set; }

        /// <summary>
        /// Название завода
        /// </summary>
        public string Factory_name { get; set; }

        /// <summary>
        /// Активность
        /// </summary>
        public bool IsActive { get; set; }
    }
}
