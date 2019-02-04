using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using PassengersCounter.Model;
using YandexAPI.Maps;
using YandexAPI;

namespace PassengersCounter.Model
{
    public class AddressPassengersSource
    {
        private readonly string path = Directory.GetCurrentDirectory();
        private string fName;       
        public List<string> address = new List<string>();
        private static readonly AddressDBEntities _dbContext = new AddressDBEntities();
        ObservableCollection<Passenger> Passengers { get; set; }
        private GeoCode _geoCode = new GeoCode();

        public AddressPassengersSource()
        {
            //var passengers = _dbContext.Passenger.ToList();
            //var busStops = _dbContext.BusStopTable.ToList();
            //var polygon = _dbContext.BusStopTable.ToList();
            //var shift = _dbContext.Shift.ToList();
            //SerializeObject(passengers, "C:\\PassengersCounter\\PassengersCounter\\passengers.bin");
            //SerializeObject(busStops, "C:\\PassengersCounter\\PassengersCounter\\BusStopTable.bin");
            //SerializeObject(polygon, "C:\\PassengersCounter\\PassengersCounter\\Polygon.bin");
            //SerializeObject(shift, "C:\\PassengersCounter\\PassengersCounter\\Shift.bin");

        }
        private void UpdatePassengers()
        {
            Passengers.Clear();
            foreach (var pass in _dbContext.Passenger.ToList())
            {
                Passengers.Add(pass);
            }
        }
        private ObservableCollection<Passenger> SetFirstColumnFont()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = xlApp.Workbooks.Add("C:\\Users\\Ильгар\\Downloads\\Список с адресами.xls");
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            
            var lastCell = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);
           
            var collPassrs = new ObservableCollection<Passenger>();
           
            for (int i = 9; i < lastCell.Row-1; i++)
            {
                collPassrs.Add(new Passenger { Shift = worksheet.Cells[i + 1, 4].Text.ToString(), Address = worksheet.Cells[i + 1, 11].Text.ToString(), CoordinatesAddress = GetPointsAddress(worksheet.Cells[i + 1, 11].Text.ToString()) });            
                
            }
            
            return collPassrs;
        }

        private string GetPointsAddress(string address)
        {
            PointD point = new PointD();
            point = _geoCode.GetPointD(_geoCode.SearchObject(address));
            double tempcoor = point.X;
            point.X = point.Y;
            point.Y = tempcoor;
            

            return (point.X.ToString().Replace(",", ".") + "," + point.Y.ToString().Replace(",", "."));
        }

        private void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, serializableObject);

            }
        }

        private List<string> DeserializeObject(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return default(List<string>); }
            if (!File.Exists(fileName)) { return default(List<string>); }
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter binForm = new BinaryFormatter();
                fs.Seek(0, SeekOrigin.Begin);
                List<string> obj = (List<string>)binForm.Deserialize(fs);
                return obj;
            }
        }

    }
}
