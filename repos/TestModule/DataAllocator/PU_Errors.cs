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
using System.Windows.Forms;

namespace DataAllocator
{
    public class PU_Errors
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
        private ObservableCollection<Leit_Errors> leitColl;
        private System.Threading.Timer timer;

        private delegate void SenderText(string lastEntry, Form form);
        SenderText senderText;
        SenderText senderState;

        public LastEntry stateModule = new LastEntry() { Message = "выключен" };        
        
        public PU_Errors()
        {
            _dbContext = new LOG_ZAVOD_NFEntities();

            try
            {
                if (_dbContext.Leit_Errors.Any())
                {

                    messageLeit1 = _dbContext.Leit_Errors.Any(x => x.PU_id == 1) ? "Leit_Errors(1). Время: " + _dbContext.Leit_Errors.Where(x => x.PU_id == 1).Max(x => x.StartTime) : new DateTime().ToString();
                    messageLeit2 = _dbContext.Leit_Errors.Any(x => x.PU_id == 2) ? "Leit_Errors(2). Время: " + _dbContext.Leit_Errors.Where(x => x.PU_id == 2).Max(x => x.StartTime) : new DateTime().ToString();
                    messageLeit3 = _dbContext.Leit_Errors.Any(x => x.PU_id == 3) ? "Leit_Errors(3). Время: " + _dbContext.Leit_Errors.Where(x => x.PU_id == 3).Max(x => x.StartTime) : new DateTime().ToString();
                }

            }
            catch (Exception ex)
            {
                Scope.WriteError("PU/PU_Errors()" + ex.Message);
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
                System.Threading.TimerCallback callback = new System.Threading.TimerCallback(WriteData);
                timer = new System.Threading.Timer(callback, null, 0, 60000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("PU_Errors - " + ex.Message);   
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
                DateTime defaultTime = Convert.ToDateTime("01.01.2000 00:00:01");

                for (int i = ipAdress.Count; i > 0; i--)
                {
                    try
                    {
                        switch (i)
                        {
                            case 1:
                                start = _dbContext.Leit_Errors.Any(x => x.PU_id == 1) ? _dbContext.Leit_Errors.Where(x => x.PU_id == 1)
                                                                                                  .Max(x => x.StartTime) : defaultTime; break;

                            case 2:
                                start = _dbContext.Leit_Errors.Any(x => x.PU_id == 2) ? _dbContext.Leit_Errors.Where(x => x.PU_id == 2)
                                                                                                  .Max(x => x.StartTime) : defaultTime; break;

                            case 3:
                                start = _dbContext.Leit_Errors.Any(x => x.PU_id == 3) ? _dbContext.Leit_Errors.Where(x => x.PU_id == 3)
                                                                                                  .Max(x => x.StartTime) : defaultTime; break;

                        }
                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError(" Поиск в _dbContext.Leit_Errors -> " + ex.Message);
                    }

                    if (DateTime.Now >= start)
                    {
                        leitColl = new ObservableCollection<Leit_Errors>();
                        DataRow[] data = new DataRow[0];

                        try
                        {
                            data = SqlQueryLeit("SELECT fehs.StartTime, fehs.Fehlernummer AS ErrorID, Kurztext AS ShortText, Langtext AS Text, fehs.FehlerquellenId AS Source, " +
                            "EndTime FROM	Fehler fehs INNER JOIN Fehlertexte fehtext ON fehs.Fehlernummer = fehtext.Fehlernummer AND SprachId = 11 AND " +
                            "fehs.FehlerquellenId = fehtext.FehlerquellenId AND '" + start.ToString("MM.dd.yyyy HH:mm:ss") + "' < fehs.StartTime ORDER BY fehs.StartTime", connStr.Replace("!", ipAdress[i - 1])
                       );
                        }
                        catch (Exception ex)
                        {
                            Scope.WriteError("data = SqlQueryLeit()" + ex.Message);
                        }

                        for (int j = 0; j < data.Count(); j++)
                        {
                            var item = data[j].ItemArray;
                            Leit_Errors error = new Leit_Errors { PU_id = i, StartTime = Convert.ToDateTime(item[0]), ErrorNumber = Convert.ToInt32(item[1]), ShortText = item[2].ToString(), LongText = item[3].ToString(), ErrorCode = Convert.ToInt32(item[4]), EndTime = item[5] != DBNull.Value ? Convert.ToDateTime(item[5]) : (DateTime?)null };

                            try
                            {
                                if (_dbContext.Leit_Errors.Any(x => x.StartTime == error.StartTime && x.ErrorNumber == error.ErrorNumber))
                                {
                                    continue;
                                }
                                else
                                {
                                    leitColl.Add(error);
                                }
                            }
                            catch (Exception ex)
                            {
                                Scope.WriteError("PU_Errors\for (int j = 0; j < data.Count(); j++) -> " + ex.Message);
                            }

                        }

                        if (leitColl != null && leitColl.Any())
                        {
                            SaveData(leitColl, i);
                        }

                    }

                }
                if (leitColl.Any() && leitColl != null)
                {

                    try
                    {
                        SendTextOnActiveForm(messageLeit1, Form.ActiveForm);
                        SendTextOnActiveForm(messageLeit2, Form.ActiveForm);
                        SendTextOnActiveForm(messageLeit3, Form.ActiveForm);
                    }
                    catch (Exception)
                    {

                    }
                }
                isRunning = false;
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("PU_Errors - " + ex.Message);
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
                if (msg.Contains("Leit_Errors(1)"))
                {
                    lastEntryOne.Message = msg;
                }
                else if (msg.Contains("Leit_Errors(2)"))
                {
                    lastEntryTwo.Message = msg;
                }
                else if (msg.Contains("Leit_Errors(3)"))
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

        private void SaveData(ObservableCollection<Leit_Errors> insertData, int index)
        {
            try
            {
                LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                Scope.BulkInsert(insertData, new SqlConnection(_context.Database.Connection.ConnectionString), "Leit_Errors");

                switch (index)
                {
                    case 1: SendTextOnActiveForm("Leit_Errors(1). Время: " + Convert.ToString(insertData.Max(x => x.StartTime)), Form.ActiveForm); break;
                    case 2: SendTextOnActiveForm("Leit_Errors(2). Время: " + Convert.ToString(insertData.Max(x => x.StartTime)), Form.ActiveForm); break;
                    case 3: SendTextOnActiveForm("Leit_Errors(3). Время: " + Convert.ToString(insertData.Max(x => x.StartTime)), Form.ActiveForm); break;
                }

            }
             catch (Exception)
            {
                //WriteError(" SaveChange in Leit_Error -> " + ex.Message);
            }

            insertData.Clear();

        }

    }

    
}
