using System.ComponentModel;

namespace Vertical.Models
{
    public class AddSystemObjectModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Имя объекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID Типа объекта
        /// </summary>
        public int? TypeID { get; set; }

        /// <summary>
        /// GUID родительского объекта
        /// </summary>
        public string ParentGUID { get; set; } 
    }
}
