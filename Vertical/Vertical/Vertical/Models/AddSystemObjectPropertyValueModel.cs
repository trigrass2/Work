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
    }
}
