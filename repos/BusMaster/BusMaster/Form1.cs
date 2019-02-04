using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using YandexAPI;
using YandexAPI.Maps;
using System.Reflection;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime.Serialization.Formatters.Binary;
using Syncfusion.Windows.Forms;

namespace BusMaster
{
    public partial class Form1 : MetroForm
    {             
        List<string> listAdres = new List<string>();
        PolygonMap polygonMap;
        GeoCode geoCode = new GeoCode();
        //PointD mainPoint;
        PointD[] points;
        string fName = @".\placemark.html";
        string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        public ChromiumWebBrowser chromeBrowser;

        public Form1()
        {
            InitializeComponent();

            //InitializeChromium();            

            SetFirstColumnFont();
            //var mainPoint = geoCode.GetPoint(geoCode.SearchObject("Обнинск"));
            string busStop1 = geoCode.GetPoint(geoCode.SearchObject("Обнинск, энгельса 22"));

            points = new PointD[] { new PointD(55.122785, 36.605280), new PointD(55.121617, 36.624077), new PointD(55.112725, 36.619872), new PointD(55.115234, 36.601375) };
            polygonMap = new PolygonMap("1", points);
            MessageBox.Show(string.Format("Обнинск: Муз. шк({0})", GetPassengers()));

        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();            
            Cef.Initialize(settings);
            
            chromeBrowser = new ChromiumWebBrowser(Path.Combine(appDir, @"placemark.html"));
            
            //splitContainer1.Panel1.Controls.Add(chromeBrowser);            

            chromeBrowser.Dock = DockStyle.Fill;
        }
       
        private void SetFirstColumnFont()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = xlApp.Workbooks.Add("C:\\Users\\Ильгар\\Downloads\\Список с адресами.xls");
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];            
            
            var lastCell = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);

            for (int i = 9; i < 100; i++)
            {               
                listAdres.Add(worksheet.Cells[i + 1, 11].Text.ToString());
            }

        }

        private int GetPassengers()
        {
            var pointAddress = GetPointsAddress();
            bool b = false;
            int countPassengers = 0;
            for (int i = 0; i < pointAddress.Length; i++)
            {
                b = polygonMap.IsInPolygon(pointAddress[i]);
                if (b == true) countPassengers++; 
            }

            return countPassengers;
        }

        private PointD[] GetPointsAddress()
        {
            PointD[] points = new PointD[listAdres.Count];

            for(int i = 0; i < points.Length; i++)
            {
                points[i] = geoCode.GetPointD(geoCode.SearchObject(listAdres[i]));
                double tempcoor = points[i].X;
                points[i].X = points[i].Y;
                points[i].Y = tempcoor;
            }

            return points;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, serializableObject);

            }
        }

        private string[] DeserializeObject(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return default(string[]); }
            if (!File.Exists(fileName)) { return default(string[]); }
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter binForm = new BinaryFormatter();
                fs.Seek(0, SeekOrigin.Begin);
                string[] obj = (string[])binForm.Deserialize(fs);              return obj;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }

}
