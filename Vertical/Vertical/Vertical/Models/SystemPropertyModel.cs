using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vertical.Models
{
    /// <summary>
    /// Модель свойства объекта
    /// </summary>
    public class SystemPropertyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID свойства
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// Название свойства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID типа свойства
        /// </summary>
        public int PropertyTypeID { get; set; }

        /// <summary>
        /// Название типа свойства объекта
        /// </summary>
        public string PropertyTypeName { get; set; }

        /// <summary>
        /// Видимость свойства
        /// </summary>
        public bool Hidden { get; set; }

        ///// <summary>
        ///// Блокируется ли свойство объекта для записи
        ///// </summary>
        //public bool Lockable { get; set; }

        ///// <summary>
        ///// Вес свойства
        ///// </summary>
        //public int? Weight { get; set; }

        ///// <summary>
        ///// ID Группы к которой принадлежит свойство
        ///// </summary>
        //public int? GroupID { get; set; }

        ///// <summary>
        ///// Название группы к которой принадлежит свойство
        ///// </summary>
        //public string GroupName { get; set; }
    }
}
