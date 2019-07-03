using System;
using System.ComponentModel;

namespace Vertical.Models
{    
    public class SystemObjectModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string GUID { get; set; }
        public string Name { get; set; }
        public string ParentGUID { get; set; }
        public int? TypeID { get; set; } = null;
        public DateTime CreationTime { get; set; }
        public string TypeName { get; set; }
        public string UserGUID { get; set; }
        public string UserName { get; set; }
    }
}