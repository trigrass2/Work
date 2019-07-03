using PropertyChanged;

namespace Vertical.Models
{
    [AddINotifyPropertyChangedInterface]
    public class SystemObjectTypeModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
