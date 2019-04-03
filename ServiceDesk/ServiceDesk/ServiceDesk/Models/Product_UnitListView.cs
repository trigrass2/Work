using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Производственная единица
    /// </summary>
    public class Product_UnitListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID
        /// </summary>
        public int Unit_id { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        public string Unit_name { get; set; }

        /// <summary>
        /// ID линии
        /// </summary>
        public int Plant_id { get; set; }

        /// <summary>
        /// Название линии
        /// </summary>
        public string Plant_name { get; set; }
    }
}
