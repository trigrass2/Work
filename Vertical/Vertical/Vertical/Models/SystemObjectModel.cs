using System;
using System.ComponentModel;

namespace Vertical.Models
{    
    /// <summary>
    /// Модель объекта
    /// </summary>
    public class SystemObjectModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// GUID объекта
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Название объекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// GUID родителя объекта
        /// </summary>
        public string ParentGUID { get; set; }

        /// <summary>
        /// ID типа объекта
        /// </summary>
        public int? TypeID { get; set; } = null;

        /// <summary>
        /// Название типа объекта
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Дата создания объекта
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// GUID пользователя создавшего объект
        /// </summary>
        public string UserGUID { get; set; }

        /// <summary>
        /// Имя пользователя создавшего объект
        /// </summary>
        public string UserName { get; set; }
    }
}