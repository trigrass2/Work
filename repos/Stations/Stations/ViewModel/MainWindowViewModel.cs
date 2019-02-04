using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using GalaSoft.MvvmLight;
using Stations.Model;
using System.Reflection;
using System.Linq;
using System.Threading;
using System.Windows.Media;

namespace Stations.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        
        private string connectionString = $"data source = {Properties.Settings.Default.SERVER}; initial catalog = {Properties.Settings.Default.DataBase}; user id = {Properties.Settings.Default.UserID};password={Properties.Settings.Default.Password}";
        private ObservableCollection<JoinItems> _joinItems;
        public ObservableCollection<JoinItems> JoinsItems { get { return _joinItems; } set { _joinItems = value; RaisePropertyChanged(); } }

        private ObservableCollection<JoinItems> _stations;
        public ObservableCollection<JoinItems> Stations { get { return _stations; } set { _stations = value; RaisePropertyChanged(); } }

        private SqlDataAdapter mySqlDataAdapter;
        private Timer timer;
        private bool isRunning;
        public Logger logger = new Logger();
        public string[] stns = Properties.Settings.Default.Stations.Split(new string[] { ",", ", ", " , " }, StringSplitOptions.RemoveEmptyEntries);
        public string deadLineArm = Properties.Settings.Default.DeadLineArm;
        public string deadLineBeton = Properties.Settings.Default.DeadLineBeton;
        
        SynchronizationContext uiContext;

        public ObservableCollection<Beton> Betons { get; set; }        

        public MainWindowViewModel()
        {
            try
            {
                Stations = new ObservableCollection<JoinItems>();
                for (int i = 0; i < stns.Length; i++)
                {
                    Stations.Add(new JoinItems { Stationsnummer = int.Parse(stns[i]) });
                }
                uiContext = SynchronizationContext.Current;
                logger.Write("Start");
                Betons = new ObservableCollection<Beton>();
                TimerFunc();
            }
            catch (Exception)
            {

                throw;
            }
                       
        }

        public void UpdateBeton(ICollection<string> elements, int pallId, int stNum)
        {
            try
            {
                foreach (var e in elements)
                {
                    string query = "SELECT  cad.FK_PalettenId,st.F_Palettennummer ,e.Name, bet.Bindemittelvolumen, bet.FestigkeitsKlasse, st.Zeittype " +
                               "FROM [l2cad].[dbo].[Element] e " +
                               "INNER JOIN (SELECT [AuftragsNr]" +
                                                ",[ElementId]" +
                                                ",[Bindemittelvolumen]" +
                                                ",[FestigkeitsKlasse] " +
                               "FROM [l2cad].[dbo].[SubElement]" +
                               "  ) bet on e.AuftragsNr = bet.AuftragsNr and e.ElementId = bet.ElementId" +
                               "  inner join [l2cad].[dbo].[CADPalBelegung] cad on cad.AuftragsNr = e.AuftragsNr" +
                               "  inner join Stationszeiten st on cad.FK_PalettenId = st.PalettenID" +
                               $"  WHERE e.Name = '{e}' and cad.FK_PalettenId = {pallId} and st.Stationsnummer = {stNum}";

                    DataTable dataTable = new DataTable();

                    dataTable = SQLQuery(connectionString, query);

                    Betons.Add(new ObservableCollection<Beton>(ToListof<Beton>(dataTable)).FirstOrDefault());
                }
            }
            catch(Exception ex)
            {
                logger.Write("UpdateBeton() -> " + ex.Message);
            }
            
        }

        public void UpdateStations(ICollection<JoinItems> joinItems)
        {
            try
            {                
                JoinItems item = new JoinItems();
                for (int i = 0; i < joinItems.Count; i++)
                {                    
                    item = joinItems.Where(x => x.Stationsnummer == Stations[i].Stationsnummer).FirstOrDefault();
                    if(item.ArrayElements == null)
                    {                        
                        Stations[i] = new JoinItems(item,"0");
                        Stations[i].DeadLine = null;                        
                    }
                    else if(Stations[i].F_Palettennummer != item.F_Palettennummer || Stations[i].EqualsElements(item) == false)
                    {
                        Stations[i] = i <=2 ? new JoinItems(item,deadLineArm) : new JoinItems(item, deadLineBeton);
                        
                    }
                    if (Stations[i] != null && Stations[i].DeadLine != null && Stations[i].DeadLine.TimerText != null && Stations[i].DeadLine != null /*&& Stations[i].DeadLine.ColorTimer != null*/)
                    {
                        Stations[i].Color = Stations[i].DeadLine.TimerText.Contains("-") ? Brushes.Red : Brushes.White;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Write($"{DateTime.Now} in UpdateStations -> " + ex.Message);   
            }
            
        }

        public void TimerFunc()
        {
            try
            {
                TimerCallback callback = new TimerCallback(UpdateProgram);
                timer = new Timer(callback, null, 0, Properties.Settings.Default.Timer * 1000);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                logger.Write($"{ DateTime.Now} - {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                logger.Write($"{ DateTime.Now} - {ex.Message}");
            }

        }

        private void UpdateProgram(object obj)
        {
            if (isRunning) return;
            isRunning = true;
            
            UpdateJoinsItems(stns);           

            isRunning = false;
        }
        private void UpdateJoinsItems(params string[] stn)
        {            
            string whereString = string.Empty;
            
            try
            {
                Betons.Clear();
                for (int i = 0; i < stn.Count(); i++)
                {
                    whereString += $"st.Stationsnummer = {stn[i]}";
                    if (i < stn.Count() - 1) whereString += " or ";
                }
            }
            catch (Exception ex)
            {
                logger.Write($"{DateTime.Now} - {ex.Message}");   
            }

            string query = "select st.Stationsnummer, st.Zeit, st.Zeittype, st.PalettenID, st.F_Palettennummer, cadTable.ProdNr, cadTable.Elements, cadTable.BetonBindevolumen" +
                           " from Stationszeiten st" +
                           " inner join (SELECT Stationsnummer, MAX(Zeit) Zeit" +
                                        " FROM Stationszeiten" +
                                        " group by Stationsnummer" +
                                        ") stat ON st.Stationsnummer = stat.Stationsnummer and st.Zeit = stat.Zeit" +
                           " inner join(select ProdNr," +
                                              "FK_PalettenId," +
                                              "result.F_Palettennummer," +
                                              "result.BetonBindevolumen, " +
                                              "SUBSTRING((SELECT distinct ', ' + cad.AuftragsNr AS[text()] FROM [l2cad].[dbo].CADPalBelegung cad WHERE cad.AuftragsNr IN(select AuftragsNr from[l2cad].[dbo].CADPalBelegung where FK_PalettenId = result.FK_PalettenId) FOR XML PATH('')),2,1000) as Auftrags," +
                                              "SUBSTRING((SELECT distinct ',' + elements.Name AS[text()] FROM (SELECT el.Name, cad.FK_PalettenId FROM [l2cad].[dbo].CADPalBelegung cad join l2cad.dbo.Element el on el.AuftragsNr = cad.AuftragsNr and el.ElementId = cad.ElementId) as elements" +
                                                         " WHERE elements.FK_PalettenId = result.FK_PalettenId FOR XML PATH('')),2,1000) as Elements" +
                                      " FROM(SELECT pal.ProdNr as ProdNr," +
                                                   "cad.FK_PalettenId as FK_PalettenId," +
                                                   "pal.Produktionsreihenfolge as Produktionsreihenfolge," +
                                                   "pal.F_Palettennummer, " +
                                                   "pal.BetonBindevolumen" +
                                           " FROM [l2cad].[dbo].CADPalBelegung cad" +
                                           " INNER JOIN [l2prod].[dbo].[Palette] pal ON pal.PalettenID = cad.FK_PalettenId" +
                                           " GROUP BY pal.ProdNr, cad.FK_PalettenId, pal.Produktionsreihenfolge, pal.F_Palettennummer, pal.BetonBindevolumen) as result" +
                                           ") as cadTable" +
                           " ON st.PalettenID = cadTable.FK_PalettenId" +
                           $" where {whereString}";
            try
            {
                DataTable dataTable = new DataTable();

                dataTable = SQLQuery(connectionString, query);

                JoinsItems = new ObservableCollection<JoinItems>(ToListof<JoinItems>(dataTable));

                uiContext.Send(x => UpdateStations(JoinsItems), null);

                foreach (var s in Stations)
                {
                    if (s.ArrayElements != null)
                    {
                        UpdateBeton(s.ArrayElements, s.PalettenID, s.Stationsnummer);
                    }
                }

                for (int i = 0; i < Stations.Count; i++)
                {
                   
                    Stations[i].ElementsAndBetons = new Dictionary<string, string>();

                    if (Stations[i].ArrayElements != null)
                    {
                        for (int j = 0; j < Stations[i].ArrayElements.Count; j++)
                        {
                            Stations[i].ElementsAndBetons.Add(Stations[i].ArrayElements[j], Betons.Where(x => x.Name == Stations[i].ArrayElements[j]).Select(k => k.Bindemittelvolumen + "; " + k.FestigkeitsKlasse).FirstOrDefault());
                        }
                    }
                    else Stations[i].ElementsAndBetons.Clear();
                }
            }
            catch (Exception ex)
            {

                logger.Write($"{DateTime.Now} - {ex.Message}");
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
            catch (Exception ex)
            {
                logger.Write($"{DateTime.Now} - {ex.Message}");
                return new DataTable();
            }

        }        

        public static List<T> ToListof<T>(DataTable dt)
        {
            try
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
            catch (Exception ex)
            {               
                throw;
            }
            
        }       

    }
}
