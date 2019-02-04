using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Stations.Model
{
    public class JoinItems : ViewModelBase
    {
        private int _prodNr;        
        private string _elements;
        private int _palettenId;
        private int? _f_Palettennummer;
        private int _stationsNummer;
        private DateTime _zeit;
        private short _zeitType;
        private double _betonBindevolumen;
        private DeadLine _deadLine;
        private Brush _color = Brushes.White;
        
        public Brush Color
        {
            get
            {
                return _color;
            }
            set
            {                
                _color = value;              
                
                RaisePropertyChanged();
            }
        }
        
        public JoinItems() { }

        public JoinItems(JoinItems item,string startTime)
        {
            _prodNr = item.ProdNr;
            _elements = item.Elements;
            _palettenId = item.PalettenID;
            _f_Palettennummer = item.F_Palettennummer;
            _stationsNummer = item.Stationsnummer;
            _zeit = item._zeit;
            _zeitType = item._zeitType;
            _betonBindevolumen = item.BetonBindevolumen;
            _deadLine = new DeadLine(startTime);            
            _color = item.Color;
            _elementsAndBetons = item.ElementsAndBetons;
        }

        public double BetonBindevolumen
        {
            get
            {
                return _betonBindevolumen;
            }
            set
            {
                _betonBindevolumen = value;
                RaisePropertyChanged();                
            }
        }

        public short Zeittype
        {
            get
            {
                return _zeitType;
            }
            set
            {
                _zeitType = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Zeit
        {
            get
            {
                return _zeit;
            }
            set
            {
                _zeit = value;
                RaisePropertyChanged();
            }
        }

        public int Stationsnummer
        {
            get
            {
                return _stationsNummer;
            }
            set
            {
                _stationsNummer = value;
                RaisePropertyChanged();
            }
        }

        public int PalettenID
        {
            get
            {
                return _palettenId;
            }
            set
            {
                _palettenId = value;
                RaisePropertyChanged();
            }
        }

        public int? F_Palettennummer
        {
            get
            {
                return _f_Palettennummer;
            }
            set
            {
                _f_Palettennummer = value;
                RaisePropertyChanged();
            }
        }

        public int ProdNr
        {
            get { return _prodNr%200; }
            set
            {
                _prodNr = value;
                RaisePropertyChanged();
            }
        }
        
        public string Elements
        {
            get { return _elements; }
            set
            {
                _elements = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> _arrayElements;
        public ObservableCollection<string> ArrayElements
        {
            get
            {
                try
                {
                    if (_zeitType == 1 || _zeitType == 4)
                    {
                        return _elements != null ? new ObservableCollection<string>(_elements.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)) : _arrayElements;
                    }
                    else return null;
                }
                catch (Exception)
                {
                    throw;                   
                }
   
            }
            set
            {
                _arrayElements = value;
                RaisePropertyChanged();
            }        
        }

        private ObservableCollection<string> _betons;
        public ObservableCollection<string> Betons
        {
            get
            {
                return _betons;
            }
            set
            {
                _betons = value;
                RaisePropertyChanged();                
            }
        }

        private Dictionary<string,string> _elementsAndBetons;
        public Dictionary<string,string> ElementsAndBetons
        {
            get
            {
                return _elementsAndBetons;
            }
            set
            {
                _elementsAndBetons = value;
                RaisePropertyChanged();
            }
        }

        public DeadLine DeadLine
        {
            get
            {
                return _deadLine;
            }
            set
            {
                _deadLine = value;
                RaisePropertyChanged();
            }
        }

        public bool EqualsElements(JoinItems item)
        {
            return item.Elements != _elements ? false : true;           
        }
    }
}
