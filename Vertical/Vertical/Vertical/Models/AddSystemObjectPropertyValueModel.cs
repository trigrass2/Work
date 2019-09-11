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
                if(obj is AddSystemObjectPropertyValueModel item)
                {
                    if (ObjectGUID == item.ObjectGUID && PropertyID == item?.PropertyID && PropertyNum == item?.PropertyNum && Value == item?.Value && ValueNum == item?.ValueNum)
                    {
                        return true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Loger.WriteMessageAsync(Android.Util.LogPriority.Error, "При сравнении двух свойств -> ",ex.Message);
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
