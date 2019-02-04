using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AddingProductUnit
{
    public class ProductUnit : INotifyPropertyChanged
    {
        private int _unit_id { get; set; }
        private string _name { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Unit_id
        {
            get { return _unit_id; }
            set
            {
                _unit_id = value;
                OnPropertyChanged("Unit_Id");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
