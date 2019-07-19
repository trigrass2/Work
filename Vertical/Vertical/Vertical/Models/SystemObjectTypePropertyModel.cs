using System.ComponentModel;

namespace Vertical.Models
{
    public class SystemObjectTypePropertyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID свойства
        /// </summary>
        public int? PropertyID { get; set; }

        /// <summary>
        /// Номер привязки
        /// </summary>
        public int? PropertyNum { get; set; }

        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

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

        /// <summary>
        /// Блокируется ли свойство объекта для записи
        /// </summary>
        public bool Lockable { get; set; }

        /// <summary>
        /// Вес свойства
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// ID Группы к которой принадлежит свойство
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// Название группы к которой принадлежит свойство
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// GUID Объекта-источника
        /// </summary>
        public string SourceObjectGUID { get; set; }
    }
}
