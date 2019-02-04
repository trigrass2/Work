using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAllocator
{
    public class PULeitToMES
    {        
        private static object locker = new object();
        private static List<string> ipAdress = new List<string>() { "33", "65", "97" };
        private static bool isRunning;
        private readonly string connStr = ConfigurationManager.ConnectionStrings["PULeit_Connection_String"].ConnectionString;
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private SqlDataAdapter mySqlDataAdapter;
        private DataTable myDataTable;
        private readonly LOG_ZAVOD_NFEntities _dbContext;
        public LastEntry lastEntryOne;
        public LastEntry lastEntryTwo;
        public LastEntry lastEntryThree;
        private string messageLeit1 = "";
        private string messageLeit2 = "";
        private string messageLeit3 = "";
        private ObservableCollection<Leit_PU> leitColl;
        private System.Threading.Timer timer;

        private delegate void SenderText(string lastEntry, Form form);
        SenderText senderText;
        SenderText senderState;

        public LastEntry stateModule = new LastEntry() { Message = "выключен" };

        public PULeitToMES()
        {

            _dbContext = new LOG_ZAVOD_NFEntities();

            try
            {
                if (_dbContext.Leit_PU.Any())
                {
                    
                    messageLeit1 = "Leit_PU(1). Время: " + _dbContext.Leit_PU.Where(x => x.PU_id == 1).Max(x => x.Timestamp);
                    messageLeit2 = "Leit_PU(2). Время: " + _dbContext.Leit_PU.Where(x => x.PU_id == 2).Max(x => x.Timestamp);
                    messageLeit3 = "Leit_PU(3). Время: " + _dbContext.Leit_PU.Where(x => x.PU_id == 3).Max(x => x.Timestamp);
                }
                
            }
            catch (Exception ex)
            {
                Scope.WriteError("PU/PULeitToMES()" + ex.Message);
            }

            lastEntryOne = new LastEntry() { Message = messageLeit1 };
            lastEntryTwo = new LastEntry() { Message = messageLeit2 };
            lastEntryThree = new LastEntry() { Message = messageLeit3 };
            senderText = new SenderText(SendTextOnActiveForm);
            senderState = new SenderText(SendStateOnActiveForm);           

        }

        public void StartModule(object sender, DoWorkEventArgs e)
        {            

            try
            {
                SendStateOnActiveForm("включен", Form.ActiveForm);
            }
            catch (Exception)
            {

            }

            try
            {
                TimerCallback callback = new TimerCallback(WriteData);
                timer = new System.Threading.Timer(callback, null, 0, 50000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("PULeitToMES - " + ex.Message);
            }

            
        }

        public void StopModule()
        {
            try
            {
                SendStateOnActiveForm("выключен", Form.ActiveForm);
            }
            catch (Exception) { }
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, 0);
                timer.Dispose();
            }
        }

        private void WriteData(object obj)
        {
            try
            {
                if (isRunning) return;

                isRunning = true;

                DateTime start = new DateTime();

                for (int i = ipAdress.Count; i > 0; i--)
                {
                    try
                    {
                        switch (i)
                        {
                            case 1:
                                start = _dbContext.Leit_PU.Any(x => x.PU_id == 1) ? _dbContext.Leit_PU.Where(x => x.PU_id == 1)
                                                                                                       .Max(x => x.Timestamp) : DateTime.Now.AddDays(-90); break;

                            case 2:
                                start = _dbContext.Leit_PU.Any(x => x.PU_id == 2) ? _dbContext.Leit_PU.Where(x => x.PU_id == 2)
                                                                                                       .Max(x => x.Timestamp) : DateTime.Now.AddDays(-90); break;

                            case 3:
                                start = _dbContext.Leit_PU.Any(x => x.PU_id == 3) ? _dbContext.Leit_PU.Where(x => x.PU_id == 3)
                                                                                                                 .Max(x => x.Timestamp) : DateTime.Now.AddDays(-90); break;

                        }
                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError(" Поиск в _dbContext.Leit_PU -> " + ex.Message);
                    }
                    if (start == null)
                    {
                        start = DateTime.Now.AddDays(-90);
                    }

                    if (DateTime.Now >= start)
                    {
                        var lastLeits = _dbContext.Leit_PU.Where(x => x.PU_id == i && x.Timestamp == start).ToList();

                        leitColl = new ObservableCollection<Leit_PU>();
                        DataRow[] data = new DataRow[0];
                        try
                        {
                            data = SqlQueryLeit(
                           "SELECT  stat.Stationsnummer, stat.Zeit, stat.PalettenID, el.Name, stat.Zeittype FROM  Stationszeiten stat " +
                           "INNER JOIN Palettenbelegung pal ON stat.PalettenID = pal.PalettenID " +
                           "INNER JOIN[l2cad].[dbo].[Element] el ON pal.FK_AuftragsNr = el.AuftragsNr AND pal.FK_ElementID = el.ElementId WHERE (Zeittype = 5 OR Zeittype = 1) AND Zeit >= '" + start.ToString("MM.dd.yyyy HH:mm:ss.fff") + "' " +
                           "GROUP BY stat.Stationsnummer, stat.Zeit, stat.Zeittype, stat.PalettenID, el.Name ORDER BY stat.Zeit", connStr.Replace("!", ipAdress[i - 1])
                           );
                        }
                        catch (Exception ex)
                        {
                            Scope.WriteError("data = SqlQueryLeit()" + ex.Message);
                        }

                        for (int j = 0; j < data.Count(); j++)
                        {
                            var item = data[j].ItemArray;

                            Leit_PU templeit = new Leit_PU { PU_id = i, StationNumber = (int)item[0], Timestamp = Convert.ToDateTime(item[1]), PalID = (int)item[2], CADUniqueId = item[3].ToString(), Timetype = Convert.ToInt32(item[4]) };
                            try
                            {

                                if (_dbContext.Leit_PU.Any(x => x.Timestamp >= templeit.Timestamp && x.CADUniqueId == templeit.CADUniqueId && x.PU_id == templeit.PU_id && x.Timetype == templeit.Timetype && x.StationNumber == templeit.StationNumber))
                                {
                                    continue;
                                }
                                else
                                {
                                    leitColl.Add(templeit);
                                }

                            }
                            catch (Exception ex)
                            {
                                Scope.WriteError("PULeitToMES\for (int j = 0; j < data.Count(); j++) -> " + ex.Message);
                            }

                        }
                        if (leitColl != null && leitColl.Any())
                        {
                            SaveData(leitColl, i);
                        }
                    }
                }
                isRunning = false;
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("PULeitToMES - " + ex.Message);
            }
            
        }
       
        private DataRow[] SqlQueryLeit(string query, string connectionStr)
        {
            mySqlConnection = new SqlConnection(connectionStr);
            mySqlConnection.Open();
            mySqlCommand = new SqlCommand(query, mySqlConnection);
            mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
            myDataTable = new DataTable();
            mySqlDataAdapter.Fill(myDataTable);
            mySqlConnection.Close();

            return myDataTable.Select();
        }

        private void SendTextOnActiveForm(string msg, Form form)
        {
            Form activeForm = form;
            if (activeForm.InvokeRequired)
            {
                activeForm.Invoke(senderText, msg, form);
                return;
            }
            if (msg != null)
            {
                if (msg.Contains("Leit_PU(1)"))
                {
                    lastEntryOne.Message = msg;
                }
                else if (msg.Contains("Leit_PU(2)"))
                {
                    lastEntryTwo.Message = msg;
                }
                else if (msg.Contains("Leit_PU(3)"))
                {
                    lastEntryThree.Message = msg;
                }
            }
        }
        private void SendStateOnActiveForm(string state, Form form)
        {
            Form activeForm = form;
            if (activeForm.InvokeRequired)
            {
                activeForm.Invoke(senderState, state, form);
                activeForm.Invoke(new Action(() => { }));
                return;
            }
            if (state != null)
            {
                stateModule.Message = state;
            }
        }       

        private void SaveData(ObservableCollection<Leit_PU> insertData, int index)
        {
            try
            {
                LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                Scope.BulkInsert(insertData, new SqlConnection(_context.Database.Connection.ConnectionString), "Leit_PU");

                switch (index)
                {
                    case 1: messageLeit1 = "Leit_PU(1). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                    case 2: messageLeit2 = "Leit_PU(2). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                    case 3: messageLeit3 = "Leit_PU(3). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                }                
            }
            catch (Exception ex)
            {
                Scope.WriteError(" SaveChange in PULeitToMES -> " + ex.Message);
            }

            try
            {
                SendTextOnActiveForm(messageLeit1, Form.ActiveForm);
                SendTextOnActiveForm(messageLeit2, Form.ActiveForm);
                SendTextOnActiveForm(messageLeit3, Form.ActiveForm);
            }
            catch (Exception) { }
            insertData.Clear();
            
        }

        private int Divider(int countArray)
        {

            List<int> allDividers = new List<int>();

            for (int i = 1; i <= countArray / i; i++)
            {
                if (countArray % i == 0)
                {
                    if (i != countArray / i) allDividers.Add(i);
                }
            }

            return allDividers.Max();
        }
        
    }
}
