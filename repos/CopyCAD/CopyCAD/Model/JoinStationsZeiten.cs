using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CopyCAD.Model
{
    public class JoinStationsZeiten : ViewModelBase
    {        
        private int _palettenID;
        private int? _palettenNummer;
        private DateTime _minZeit;
        private DateTime _maxZeit;
        private int _count;
        private string _elementName;        

        public int PalettenID
        {
            get { return _palettenID; }
            set
            {
                _palettenID = value;
                RaisePropertyChanged();                
            }
        }

        public int? PalettenNummer
        {
            get { return _palettenNummer; }
            set
            {
                _palettenNummer = value;
                RaisePropertyChanged();
            }
        }

        public DateTime MinZeit
        {
            get { return _minZeit; }
            set
            {
                _minZeit = value;                
                RaisePropertyChanged();
            }
        }

        public DateTime MaxZeit
        {
            get { return _maxZeit; }
            set
            {
                _maxZeit = value;
                RaisePropertyChanged();
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                RaisePropertyChanged();
            }
        }

        public string ElementName
        {
            get { return _elementName; }
            set
            {
                _elementName = value;
                RaisePropertyChanged();
            }
        }
    }
}
