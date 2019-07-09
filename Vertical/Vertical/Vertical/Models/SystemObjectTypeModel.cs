using PropertyChanged;

namespace Vertical.Models
{
    /// <summary>
    /// Модель типа объекта
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class SystemObjectTypeModel
    {
        /// <summary>
        /// ID Типа объекта
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Название типа объекта
        /// </summary>
        public string Name { get; set; }
    }
}
