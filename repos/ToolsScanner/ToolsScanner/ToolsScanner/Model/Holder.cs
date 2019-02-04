using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ToolsScanner.Model
{
    [Serializable]
    public class Holder : INotifyPropertyChanged
    {
        private int person_id;
        private int tool_id;

        public int Tool_id
        {
            get { return tool_id; }
            set { tool_id = value; OnPropertyChanged("Tool_id"); }
        }

        public int Person_id
        {
            get { return person_id; }
            set { person_id = value; OnPropertyChanged("Person_id"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
