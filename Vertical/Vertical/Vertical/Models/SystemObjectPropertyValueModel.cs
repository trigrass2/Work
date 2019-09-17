using System;
using System.ComponentModel;

namespace Vertical.Models
{
    /// <summary>
    /// Модель значения свойства объекта
    /// </summary>
    public class SystemObjectPropertyValueModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SystemObjectPropertyValueModel()
        {

        }

        public SystemObjectPropertyValueModel(SystemObjectPropertyValueModel model)
        {
            ID = model.ID;
            Num = model.Num;
            Name = model?.Name;
            Value = model?.Value;
            ValueNum = model.ValueNum;
            SystemObjectGUID = model?.SystemObjectGUID;
            SystemObjectName = model?.SystemObjectName;
            TypeID = model.TypeID;
            TypeName = model?.TypeName;
            Timestamp = model?.Timestamp;
            UserGUID = model?.UserGUID;
            UserName = model?.UserName;
            Weight = model?.Weight;
            GroupID = model?.GroupID;
            GroupName = model?.GroupName;
            SourceObjectParentGUID = model?.SourceObjectParentGUID;
            SourceObjectTypeID = model?.SourceObjectTypeID;
            Locked = !model?.Locked;
            Array = model?.Array;
        }

        /// <summary>
        /// ID Свойства объекта
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Номер привязки cвойства объекта
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// Название свойства объекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение свойства
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Номер значения свойства объекта
        /// </summary>
        public int ValueNum { get; set; }

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
        public string TypeName { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? Timestamp { get; set; }

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

        /// <summary>
        /// GUID Объекта-источника
        /// </summary>
        public string SourceObjectParentGUID { get; set; }

        /// <summary>
        /// ID типа объекта-источника
        /// </summary>
        public int? SourceObjectTypeID { get; set; }

        private bool? _locked;
        /// <summary>
        /// Заблокирован для редактирования
        /// </summary>
        public bool? Locked
        {
            get
            {
                return _locked;
            }
            set
            {
                _locked = !value;
            }
        }

        /// <summary>
        /// Is Array
        /// </summary>
        public bool? Array { get; set; }
    }
}
