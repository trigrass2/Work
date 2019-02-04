using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace Stations.Model
{
    public class Beton:ViewModelBase
    {
        private string _name;
        private double _bindemittelvolumen;
        private string _festigkeitsKlasse;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public double Bindemittelvolumen
        {
            get
            {
                return Math.Round(_bindemittelvolumen,3);
            }
            set
            {
                _bindemittelvolumen = value;
                RaisePropertyChanged();
            }
        }

        public string FestigkeitsKlasse
        {
            get
            {
                return _festigkeitsKlasse;
            }
            set
            {
                _festigkeitsKlasse = value;
                RaisePropertyChanged();
            }
        }
    }
}
