using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AddingProductUnit
{
    [Serializable]
    public class ProductRegion : INotifyPropertyChanged
    {
        private int _region_id;
        private string _name;
        private int _unit_id;

        public int Region_id
        {
            get { return _region_id; }
            set
            {
                _region_id = value;
                OnPropertyChanged("Region_id");
            }
        }

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
                OnPropertyChanged("Unit_id");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
