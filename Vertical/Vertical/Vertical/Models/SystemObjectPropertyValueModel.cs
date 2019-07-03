using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vertical.Models
{
    public class SystemObjectPropertyValueModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID Свойства объекта
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Название свойства объекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение свойства
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// GUID Объекта к которому принадлежит свойство
        /// </summary>
        public string SystemObjectGUID { get; set; }

        /// <summary>
        /// Название объекта к которому принадлежит свойство
        /// </summary>
        public string SystemObjectName { get; set; }

        /// <summary>
        /// ID Типа свойства объекта
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Название типа свойства объекта
        /// </summary>
        public string Typename { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// GUID Пользователя сделавшего изменение
        /// </summary>
        public string UserGUID { get; set; }

        /// <summary>
        /// Имя пользователя сделавшего изменение
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Вес параметра
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// ID Группы к которой принадлежит параметр (для группировки)
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// Название группы к которой принадлежит параметр
        /// </summary>
        public string GroupName { get; set; }

    }
}
