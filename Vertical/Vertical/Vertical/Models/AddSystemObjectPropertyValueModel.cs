using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vertical.Models
{
    public class AddSystemObjectPropertyValueModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ObjectGUID { get; set; }
        public int? PropertyID { get; set; }
        public int? PropertyNum { get; set; }
        public object Value { get; set; }
        public int? ValueNum { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as AddSystemObjectPropertyValueModel;
            if (ObjectGUID == item.ObjectGUID && PropertyID == item.PropertyID && PropertyNum == item.PropertyNum && Value.Equals(item.Value) && ValueNum == item.ValueNum)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
