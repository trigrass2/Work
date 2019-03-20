using System.ComponentModel;

namespace ServiceDesk.Models
{
    public class Product_FactoryListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
