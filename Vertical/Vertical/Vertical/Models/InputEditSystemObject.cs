using System.ComponentModel;

namespace Vertical.Models
{
    /// <summary>
    /// Входные данные для редактирования системных объектов
    /// </summary>
    public class InputEditSystemObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// GUID объекта
        /// </summary>
        public string ObjectGUID { get; set; }

        /// <summary>
        /// Имя объекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Видимость
        /// </summary>
        public bool Hidden { get; set; } = false;

        
    }
}
