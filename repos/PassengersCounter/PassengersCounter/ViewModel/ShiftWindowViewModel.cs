using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassengersCounter.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace PassengersCounter.ViewModel
{
    public class ShiftWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Shift> Shifts { get; set; }
        private readonly AddressDBEntities _dbContext = new AddressDBEntities();

        public ShiftWindowViewModel()
        {
            Shifts = new ObservableCollection<Shift>();
            UpdateShift();
        }

        private void UpdateShift()
        {
            Shifts.Clear();
            foreach (var s in _dbContext.Shift.ToList())
            {
                Shifts.Add(s);
            }
        }

        private string[] _selectShift;
        public string[] SelectShift
        {
            get { return _selectShift; }
            set
            {
                _selectShift = value;
                RaisePropertyChanged();
            }
        }
    }
}
