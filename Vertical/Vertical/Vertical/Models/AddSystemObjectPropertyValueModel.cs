using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Vertical.Services;

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
            try
            {
                var item = obj as AddSystemObjectPropertyValueModel;
                if (ObjectGUID == item.ObjectGUID && PropertyID == item?.PropertyID && PropertyNum == item?.PropertyNum && Value.Equals(item.Value) && ValueNum == item.ValueNum)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, "При сравнении двух свойств -> ",ex.Message);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int valueHashCode = this.Value is null ? 0 : this.Value.GetHashCode();
            return this.ObjectGUID.GetHashCode() ^ this.PropertyID.GetHashCode() ^ this.PropertyNum.GetHashCode() ^ valueHashCode ^ this.ValueNum.GetHashCode();
        }
    }
}
