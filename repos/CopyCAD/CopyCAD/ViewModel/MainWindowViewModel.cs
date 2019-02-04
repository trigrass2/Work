using CopyCAD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Data;
using System.Collections;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace CopyCAD.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly string directoryAWM_one = ConfigurationManager.ConnectionStrings["directoryAWM_1"].ConnectionString;
        private readonly string directoryAWM_two = ConfigurationManager.ConnectionStrings["directoryAWM_2"].ConnectionString;
        private readonly string directoryAWM_tree = ConfigurationManager.ConnectionStrings["directoryAWM_3"].ConnectionString;

        private readonly string connectionStringAWM_one_from_l2Cad = ConfigurationManager.ConnectionStrings["connectionToL2Cad_AWM_1"].ConnectionString;
        private readonly string connectionStringAWM_two_from_l2Cad = ConfigurationManager.ConnectionStrings["connectionToL2Cad_AWM_2"].ConnectionString;
        private readonly string connectionStringAWM_tre_from_l2Cad = ConfigurationManager.ConnectionStrings["connectionToL2Cad_AWM_3"].ConnectionString;

        private readonly string connectionStringAWM_one_from_l2Prod = ConfigurationManager.ConnectionStrings["connectionToL2Prod_AWM_1"].ConnectionString;
        private readonly string connectionStringAWM_two_from_l2Prod = ConfigurationManager.ConnectionStrings["connectionToL2Prod_AWM_2"].ConnectionString;
        private readonly string connectionStringAWM_tre_from_l2Prod = ConfigurationManager.ConnectionStrings["connectionToL2Prod_AWM_3"].ConnectionString;

        private static object _lock = new object();
        private SaveFileDialog saveFileDialog = new SaveFileDialog();

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders { get { return _orders; } set { _orders = value; RaisePropertyChanged(); } }

        private ObservableCollection<JoinStationsZeiten> _joinStationsZeitens;
        public ObservableCollection<JoinStationsZeiten> JoinStationsZeitens { get { return _joinStationsZeitens; } set { _joinStationsZeitens = value; RaisePropertyChanged(); } }

        private ObservableCollection<string> _namesFiles;
        public ObservableCollection<string> NamesFiles { get { return _namesFiles; } set { _namesFiles = value; RaisePropertyChanged(); } }        
        private SqlDataAdapter mySqlDataAdapter;            

        private List<string> _listAWM;
        public List<string> ListAWM { get { return _listAWM; } set { _listAWM = value; RaisePropertyChanged(); } }


        private string _stationNummer;
        public string StationNummer { get { return _stationNummer; } set { _stationNummer = value; RaisePropertyChanged(); } }

        public MainWindowViewModel()
        {
            StationNummer = string.Empty;
            
            ListAWM = new List<string> { "AWM 1", "AWM 2", "AWM 3" };
            Orders = new ObservableCollection<Order>();
            NamesFiles = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(Orders, _lock);
            JoinStationsZeitens = new ObservableCollection<JoinStationsZeiten>();
            CountOrders = new int();
            IsEnabled = false;
        }

        private int _countOrders;
        public int CountOrders { get { return _countOrders; } set { _countOrders = value; RaisePropertyChanged(); } }

        private bool _isEnabled;
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged(); } }

        private IList _selectedOrders;
        public IList SelectedOrders
        {
            get { return _selectedOrders; }
            set
            {
                _selectedOrders = value;
                NamesFiles.Clear();
                RaisePropertyChanged("SelectedOrders");
                CountOrders = _selectedOrders.Count;
                if (CountOrders == 0)
                {
                    IsEnabled = false;
                }
                else IsEnabled = true;

                foreach (var s in _selectedOrders)
                {
                    var o = s as Order;
                    
                    o.Elements.Split(',').ToList().ForEach(x => NamesFiles.Add(x));
                }
            }
        }
        private IList _selectedStationsZeiten;
        public IList SelectedStationsZeiten
        {
            get { return _selectedStationsZeiten; }
            set
            {
                _selectedStationsZeiten = value;
                NamesFiles.Clear();
                RaisePropertyChanged();
                CountOrders = _selectedStationsZeiten.Count;
                if (CountOrders == 0)
                {
                    IsEnabled = false;
                }
                else IsEnabled = true;

                foreach (var s in _selectedStationsZeiten)
                {
                    var o = s as JoinStationsZeiten;                   
                    o.ElementName.Split(',').ToList().ForEach(x => NamesFiles.Add(x));
                }
            }
        }
        private RelayCommand _updateContentCommand;
        public ICommand UpdateContentCommand
            => _updateContentCommand ?? (_updateContentCommand = new RelayCommand(OnUpdateContentCommand));
        public void OnUpdateContentCommand()
        {
            if(SelectAWM != null)
            {                
                GetAWM(SelectAWM);
                OnSendStationsNummer();
            }            
        }
        private DataTable SQLQuery(string connectionString, string query)
        {
            try
            {
                DataTable table = new DataTable();
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = connectionString;

                    SqlCommand command = new SqlCommand(query, cn);
                    cn.Open();
                    mySqlDataAdapter = new SqlDataAdapter(command);
                    mySqlDataAdapter.Fill(table);
                    cn.Close();
                    return table;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new DataTable();
            }
           
        }
        
        private RelayCommand _sendStationsNummer;
        public ICommand SendStationsNummer
            => _sendStationsNummer ?? (_sendStationsNummer = new RelayCommand(OnSendStationsNummer));
        public void OnSendStationsNummer()
        {            
            if(StationNummer != null && StationNummer != string.Empty)
            {
                string stNummer = StationNummer.Replace(",", " or Stationsnummer=");
                string query = string.Format("select *,SUBSTRING(" +
                                                        "(SELECT distinct ', '+elements.Name AS [text()]" +
                                                        "FROM (SELECT el.Name, PalettenID FROM Palettenbelegung join l2cad.dbo.Element el on el.AuftragsNr=FK_AuftragsNr and el.ElementId=FK_ElementID) as elements " +
                                                        "WHERE elements.PalettenID=tableJoinStAndPal.PalettenID FOR XML PATH ('')" +
                                                        "), 2, 1000) as ElementName " +
                                             "from(SELECT st.PalettenID as PalettenID,F_Palettennummer as PalettenNummer ,MIN(Zeit) as MinZeit, MAX(Zeit) as MaxZeit, COUNT(st.PalettenID) as Count " +
                                                  "from Stationszeiten st " +
                                                  "where Zeittype=5 and (Stationsnummer ={0}) " +
                                                  "group by st.PalettenID, st.F_Palettennummer" +
                                                  ") as tableJoinStAndPal " +
                                              "order by MinZeit", stNummer);
                
                if (SelectAWM != null)
                {
                    List<JoinStationsZeiten> list = new List<JoinStationsZeiten>();
                    switch (SelectAWM)
                    {
                        case "AWM 1": list = ToListof<JoinStationsZeiten>(SQLQuery(connectionStringAWM_one_from_l2Prod, query)); break;
                        case "AWM 2": list = ToListof<JoinStationsZeiten>(SQLQuery(connectionStringAWM_two_from_l2Prod, query)); break;
                        case "AWM 3": list = ToListof<JoinStationsZeiten>(SQLQuery(connectionStringAWM_tre_from_l2Prod, query)); break;
                    }
                    JoinStationsZeitens = new ObservableCollection<JoinStationsZeiten>(list);
                }                
            }
        }
        
        private RelayCommand _copyFilesCommand;
        public ICommand CopyFilesCommand
            => _copyFilesCommand ?? (_copyFilesCommand = new RelayCommand(OnCreateExcelCommand));
        public void OnCreateExcelCommand()
        {
            if (NamesFiles.Count != 0)
            {
                List<string> needFiles = new List<string>();
                string directory = string.Empty;
                if(SelectAWM != null)
                {
                    switch (SelectAWM)
                    {
                        case "AWM 1": directory = directoryAWM_one; break;
                        case "AWM 2": directory = directoryAWM_two; break;
                        case "AWM 3": directory = directoryAWM_tree; break;
                    }
                }
                
                foreach (var n in NamesFiles)
                {
                    string[] findFiles = Directory.GetFiles(directory, n.Trim(), SearchOption.AllDirectories);
                    if (findFiles.Length != 0)
                        needFiles.Add(findFiles[findFiles.Length - 1]);
                }

                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                try
                {
                    if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (var f in needFiles)
                        {
                            File.Copy(f, folderBrowserDialog.SelectedPath.ToString() + "\\" + Path.GetFileName(f)+".uni");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }        

        public static List<T> ToListof<T>(DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        private string _selectAWM;
        public string SelectAWM
        {
            get
            {
                return _selectAWM;
            }
            set
            {
                _selectAWM = value;
                RaisePropertyChanged();
                GetAWM(_selectAWM);
                if (StationNummer != null && StationNummer != string.Empty)
                    OnSendStationsNummer();
            }
        }       

        public void GetAWM(string selectAWM)
        {
            Orders.Clear();
            string query = "select top (500) ProdNr, " +
                                            "FK_PalettenId, " +                                            
                                            "SUBSTRING((SELECT distinct ', '+cad.AuftragsNr AS [text()]	FROM CADPalBelegung cad WHERE cad.AuftragsNr IN (select AuftragsNr from CADPalBelegung where FK_PalettenId = result.FK_PalettenId) FOR XML PATH ('')),2,1000) as Auftrags, " +
                                            "SUBSTRING((SELECT distinct ', '+elements.Name AS [text()] FROM (	SELECT el.Name, cad.FK_PalettenId FROM CADPalBelegung cad join l2cad.dbo.Element el on el.AuftragsNr=cad.AuftragsNr and el.ElementId=cad.ElementId) as elements " +
                                            "WHERE elements.FK_PalettenId=result.FK_PalettenId FOR XML PATH ('')),2,1000) as Elements " +
                                            "FROM(" +
                                                  "SELECT pal.ProdNr as ProdNr, " +
                                                         "cad.FK_PalettenId as FK_PalettenId, " +
                                                         "pal.Produktionsreihenfolge as Produktionsreihenfolge " +
                                                  "FROM CADPalBelegung cad " +
                                                  "INNER JOIN[l2prod].[dbo].[Palette] pal ON pal.PalettenID = cad.FK_PalettenId	" +
                                                  "GROUP BY pal.ProdNr, cad.FK_PalettenId, pal.Produktionsreihenfolge)as result " +
                                                  "ORDER BY Produktionsreihenfolge desc ";
            if (selectAWM != null)
            {
                DataTable dataTable = new DataTable();
                switch (selectAWM)
                {
                    case "AWM 1": dataTable = SQLQuery(connectionStringAWM_one_from_l2Cad, query); break;
                    case "AWM 2": dataTable = SQLQuery(connectionStringAWM_two_from_l2Cad, query); break;
                    case "AWM 3": dataTable = SQLQuery(connectionStringAWM_tre_from_l2Cad, query); break;
                }
                var listOrders = ToListof<Order>(dataTable);
                listOrders.Reverse();
                for(int i = 0; i < listOrders.Count(); i++)
                {
                    listOrders[i].ProdNr = listOrders[i].ProdNr == 0 ? (listOrders[i - 1].ProdNr + 1) % 200 : listOrders[i].ProdNr % 200;
                    if (listOrders[i].ProdNr == 0) listOrders[i].ProdNr = 200;
                }
                Orders = new ObservableCollection<Order>(listOrders);
            }
        }
    }   
}
