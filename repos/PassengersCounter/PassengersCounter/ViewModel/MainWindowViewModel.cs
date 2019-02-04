using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassengersCounter.Model;
using System.Collections.ObjectModel;
using YandexAPI;
using YandexAPI.Maps;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PassengersCounter.View;
using System.Windows.Input;

namespace PassengersCounter.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AddressDBEntities _dbContext = new AddressDBEntities();
        private GeoCode _geoCode = new GeoCode();
        private PolygonMap polygonMap;        

        public ObservableCollection<BusStopTable> BusStopTables { get; set; }
        public ObservableCollection<Passenger> Passengers { get; set; }
        public ObservableCollection<Passenger> RemainderPassenger { get; set; }
        public ObservableCollection<Polygon> Polygons { get; set; }
        public ObservableCollection<Shift> Shifts { get; set; }
        public ObservableCollection<SelectedShift> SelectedShifts { get; set; }   
        public ObservableCollection<Passenger> passengersNoShifts { get; set; } 
               
        public ObservableCollection<string> Citys { get { return new ObservableCollection<string> { "Обнинск", "Малоярославец", "Балабаново", "Боровск" }; } }
        private int _allPassengers;
        public int AllPassengers { get { return _allPassengers; } set { _allPassengers = value;RaisePropertyChanged(); } }
        private int _countSelectedCityPassengers;
        public int CountSelectedCityPassengers { get { return _countSelectedCityPassengers; } set { _countSelectedCityPassengers = value; RaisePropertyChanged(); } }
        private int _countRemainderPassengers;
        public int CountRemainderPassengers { get { return _countRemainderPassengers; } set { _countRemainderPassengers = value;RaisePropertyChanged(); } }
        
        public List<Passenger> armaturnye = new List<Passenger>();

        public MainWindowViewModel()
        {            
            Passengers = new ObservableCollection<Passenger>();
            BusStopTables = new ObservableCollection<BusStopTable>();
            RemainderPassenger = new ObservableCollection<Passenger>();
            Polygons = new ObservableCollection<Polygon>();
            Shifts = new ObservableCollection<Shift>();
            SelectedShifts = new ObservableCollection<SelectedShift>();
            passengersNoShifts = new ObservableCollection<Passenger>();            
            CountRemainderPassengers = new int();            
            AllPassengers = new int();            
            UpdateShift();            
            UpdatePolygon();            
        }

        private void UpdateShift()
        {
            SelectedShifts.Clear();
            foreach(var s in _dbContext.Shift.ToList())
            {
                SelectedShifts.Add(new SelectedShift { isSelected = false, Shift = s});
            }
        }

        private void UpdatePolygon()
        {
            Polygons.Clear();
            foreach(var p in _dbContext.Polygon.ToList())
            {
                Polygons.Add(p);
            }
        }

        private void UpdatePassengers(List<string> needShifts)
        {
            Passengers.Clear();
            foreach (var sh in needShifts)
            {
                foreach (var passenger in _dbContext.Passenger.Where(x => x.Shift.Contains(sh)).ToList())
                {
                    Passengers.Add(passenger);
                }
            }            

            AllPassengers = Passengers.Count();
        }
        private void UpdateCountSelectedCityPassengers(string cityName)
        {
            CountSelectedCityPassengers = 0;
            foreach (var busStop in _dbContext.BusStopTable.Where(x => x.nameBusStop.Contains(cityName)))
            {
                CountSelectedCityPassengers += busStop.countPassengers;               
            }
        }
        private void UpdateBusStopes(string cityName)
        {            
            BusStopTables.Clear();
            RemainderPassenger.Clear();
            foreach (var busStop in _dbContext.BusStopTable.Where(x => x.nameBusStop.Contains(cityName)))
            {         
                busStop.countPassengers = GetPassengers(busStop.coordinateBusStop, busStop.nameBusStop);                                
                BusStopTables.Add(busStop);                
            }
            foreach (var p in Passengers.Where(x => x.busStop == null && x.Address.Contains(cityName)))
            {
                RemainderPassenger.Add(p);
            }
            CountRemainderPassengers = RemainderPassenger.Count();    
            _dbContext.SaveChanges();
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                RaisePropertyChanged();
                if(_selectedCity != null)
                {
                    List<string> selectShifts = SelectedShifts.Where(x => x.isSelected).Select(x => x.Shift.nameShift).ToList();
                    UpdatePassengers(selectShifts);
                    UpdateBusStopes(_selectedCity);
                    UpdateCountSelectedCityPassengers(_selectedCity);
                }
            }
        }        

        private int GetPassengers(string coordinateBusStop, string addressBusStop)
        {
            var needDelete = new ObservableCollection<Passenger>();
            PointD tp = new PointD(coordinateBusStop);
            var needPolygon = Polygons.Where(x => x.namePolygon == addressBusStop).FirstOrDefault();
            PointD[] pointsPoligon = new PointD[] 
            {                
                new PointD(needPolygon.oneCoordinate),
                new PointD(needPolygon.twoCoordinate),
                new PointD(needPolygon.threeCoordinate),
                new PointD(needPolygon.fourCoordinate)
            };

            polygonMap = new PolygonMap("1", pointsPoligon);
            bool b = false;
            int countPassengers = 0;

            for (int i = 0; i < Passengers.Count; i++)
            {
                b = polygonMap.IsInPolygon(new PointD(Passengers[i].CoordinatesAddress));
                if (b == true)
                {
                    countPassengers++;
                    needDelete.Add(Passengers[i]);
                    var p = _dbContext.Passenger.ToList()[i];
                    p.busStop = addressBusStop;                    
                }                
            }         
            foreach(var p in needDelete)
            {
                Passengers.Remove(p);
            }
            return countPassengers;
        }

        //private void ExcelFunk()
        //{
        //    Microsoft.Office.Interop.Excel.Application ObjWorkExcel = new Microsoft.Office.Interop.Excel.Application(); //открыть эксель
        //    Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
        //    Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
        //    var lastCell = ObjWorkSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);//1 ячейку

        //        for (int j = 10; j < lastCell.Row; j++) // по всем строкам
        //            armaturnye.Add(new Passenger { Address = ObjWorkSheet.Cells[ j, 12].Text.ToString(), Shift= ObjWorkSheet.Cells[j, 13].Text.ToString() }); ;//считываем текст в строку

        //    ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
        //    ObjWorkExcel.Quit(); // выйти из экселя
        //    GC.Collect(); // убрать за собой
        //}

        //private void UpdateArmatura()
        //{
        //    var rrr = new ObservableCollection<Passenger>();
        //    var addr = new List<string>();
        //    addr.AddRange(armaturnye.Select(x => x.Address));
        //    foreach (var passenger in _dbContext.Passenger.ToList())
        //    {
        //        //passenger.Shift = armaturnye.Where(x => x.Address == passenger.Address).FirstOrDefault().Shift;
        //        if(addr.Contains(passenger.Address))
        //        {
        //            passenger.Shift = armaturnye.Find(x => x.Address == passenger.Address).Shift;
        //            rrr.Add(passenger);
        //        }                
        //    }
        //    _dbContext.SaveChanges();
        //}
    }
}
