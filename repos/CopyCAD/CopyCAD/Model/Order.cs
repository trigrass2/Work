using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace CopyCAD.Model
{
    public class Order : ViewModelBase
    {
        private int _prodNr;
        private string _auftrags;
        private string _elements;
        private int? _fK_PalettenId;

        public int? FK_PalettenId
        {
            get
            {
                return _fK_PalettenId;
            }
            set
            {
                _fK_PalettenId = value;
                RaisePropertyChanged();
            }
        }

        public int ProdNr
        {
            get { return _prodNr; }
            set
            {
                _prodNr = value;
                RaisePropertyChanged();
            }
        }
        public string Auftrags
        {
            get { return _auftrags; }
            set
            {
                _auftrags = value;
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

    }
}
