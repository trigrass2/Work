using PropertyChanged;
namespace Vertical.Models
{
    /// <summary>
    /// Входные данные для получения значений свойств объекта
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class InputAddSystemObjectPropertiesValues
    {
        /// <summary>
        /// GUID объекта
        /// </summary>        
        public string ObjectGUID { get; set; }

        /// <summary>
        /// ID свойства
        /// </summary>
        public int PropertyID { get; set; }

        /// <summary>
        /// Значение свойства
        /// </summary>
        public object Value { get; set; }
    }
}
