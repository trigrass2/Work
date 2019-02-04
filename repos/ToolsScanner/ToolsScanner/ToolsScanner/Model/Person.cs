using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ToolsScanner.Model
{
    [Serializable]
    public class Person : INotifyPropertyChanged
    {
        private string person_id;
        private string person_name;

        public string Person_id
        {
            get { return person_id; }
            set { person_id = value; OnPropertyChanged("Person_id"); }
        }

        public string Person_name
        {
            get { return person_name; }
            set { person_name = value; OnPropertyChanged("Person_name"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
