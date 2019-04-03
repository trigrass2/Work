using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServiceDesk.Models
{
    public class ServiceDesk_GroupUserListView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Group_id { get; set; }
        public string Group_name { get; set; }
        public string User_id { get; set; }
        public string User_name { get; set; }
    }
}
