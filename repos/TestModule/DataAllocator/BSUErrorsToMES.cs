using System;
using System.Linq;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAllocator
{
    public class BSUErrorsToMES
    {
        private readonly LOG_ZAVOD_NFEntities _dbContext;
        public ObservableCollection<BSU_Errors_Log> bsuErrors;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BSU_Connection_String"].ConnectionString;
        private MySqlConnection mySqlConnection;
        private MySqlCommand mySqlCommand;
        private MySqlDataAdapter mySqlDataAdapter;
        private DataTable myDataTable;
        private System.Threading.Timer timer;
        private static bool isRunning;
        public LastEntry lastEntry;
        private string messageBSUError = null;

        private delegate void SenderText(string lastEntry, Form form);
        SenderText senderText;        
        SenderText senderState;
        public LastEntry stateModule = new LastEntry() { Message = "выключен" };

        public BSUErrorsToMES()
        {
            _dbContext = new LOG_ZAVOD_NFEntities();
            try
            {
                if (_dbContext.BSU_Errors_Log.Any())
                {
                    var bsu = _dbContext.BSU_Errors_Log.First();
                    messageBSUError = "BSU_Errors_Log. Время: " + bsu.Date.ToShortDateString() + " " + bsu.Time.ToString();
                }
                
            }
            catch (Exception)
            {
                Scope.WriteError("BSU/BSUErrorsToMes()");
            }
            
            lastEntry = new LastEntry() { Message = messageBSUError };
            senderText = new SenderText(SendTextOnActiveForm);
            senderState = new SenderText(SendStateOnActiveForm);
        }

        public string ConvertText(string text)
        {
            return text = Encoding.GetEncoding(1251).GetString(Encoding.GetEncoding(1252).GetBytes(text));                     
        }

        private DataRow[] SqlQueryBSU(string query)
        {
            try
            {
                mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                mySqlCommand = new MySqlCommand(query, mySqlConnection);
                mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                myDataTable = new DataTable();
                mySqlDataAdapter.Fill(myDataTable);
                mySqlConnection.Close();
            }
            catch(Exception ex)
            {
                Scope.WriteError(" SqlQueryBSU -> " + ex.Message);
            }
            

            return myDataTable.Select();
        }        

        private void WriteDataToMesReport(object obj)
        {
            try
            {
                if (isRunning) return;

                isRunning = true;

                try
                {
                    DateTime nowTime = new DateTime();
                    DateTime startDateMES = new DateTime();
                    TimeSpan startTimeMES = new TimeSpan();

                    try
                    {

                        startDateMES = _dbContext.BSU_Errors_Log.Any() ? _dbContext.BSU_Errors_Log.Max(x => x.Date) : Convert.ToDateTime("2013.09.19");
                        startTimeMES = _dbContext.BSU_Errors_Log.Any() ? _dbContext.BSU_Errors_Log.Where(x => x.Date == startDateMES).Max(x => x.Time) : Convert.ToDateTime("13:32:36").TimeOfDay;

                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError(" Поиск в _dbContextMES.BSU_Errors_Log -> " + ex.Message);
                        return;
                    }

                    nowTime = DateTime.Now;


                    while (startDateMES < nowTime)
                    {

                        string month = startDateMES.Month.ToString().Length == 1 ? "0" + startDateMES.Month.ToString() : startDateMES.Month.ToString();
                        DataRow[] bsuErrorData = new DataRow[0];

                        try
                        {
                            bsuErrorData = SqlQueryBSU("SELECT * FROM wd_1_fehlermeld_" + startDateMES.Year + "_" + month + " WHERE (datum='" + startDateMES.Date.ToString("yyyy.MM.dd") + "' AND uhrzeit > '" + startTimeMES.ToString() + "') OR datum > '" + startDateMES.Date.ToString("yyyy.MM.dd") + "' ORDER BY datum, uhrzeit");

                            if (!bsuErrorData.Any())
                            {
                                string date = "01." + startDateMES.AddMonths(1).Month.ToString() + "." + startDateMES.AddMonths(1).Year.ToString();
                                startDateMES = Convert.ToDateTime(date);
                                startTimeMES = new TimeSpan();
                                continue;
                            }
                        }
                        catch (MySqlException ex)
                        {
                            Scope.WriteError(" SqlQueryBSU -> " + ex.Message);
                            startDateMES = startDateMES.AddMonths(1);
                            continue;
                        }

                        bsuErrors = new ObservableCollection<BSU_Errors_Log>();

                        for (int i = 0; i < bsuErrorData.Count(); i++)
                        {
                            var item = bsuErrorData[i].ItemArray;

                            bsuErrors.Add(new BSU_Errors_Log { Error_id = Convert.ToInt32(item[0]), Date = Convert.ToDateTime(item[1]).Date, Time = (TimeSpan)item[2], Number = (int)item[3], SpsNumber = (int)item[4], DischargePos = (int)item[5], Recpie = (int)item[6], Weighter = (int)item[7], Silo = (int)item[8], Flag = (int)item[9], IntText = item[10] != null ? ConvertText(item[10].ToString()) : "", Text = item[11] != null ? ConvertText(item[11].ToString()) : "", LongText = item[12] != null ? ConvertText(item[12].ToString()) : "", Geht = item[13].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(item[13]), Acknowledged = item[14] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[14]), DoStart = item[15] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[15]) });

                        }
                        if (bsuErrors != null && bsuErrors.Any())
                        {
                            try
                            {
                                LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                                Scope.BulkInsert(bsuErrors, new SqlConnection(_context.Database.Connection.ConnectionString), "BSU_Errors_Log");
                                messageBSUError = "BSU_Errors_Log. Время: " + Convert.ToString(bsuErrors[bsuErrors.Count - 1].Date.ToShortDateString() + " " + bsuErrors[bsuErrors.Count - 1].Time.ToString());

                                bsuErrors.Clear();
                            }
                            catch (Exception ex)
                            {
                                Scope.WriteError("BSU Save " + ex.Message);
                            }
                            try
                            {
                                SendTextOnActiveForm(messageBSUError, Form.ActiveForm);
                            }
                            catch (Exception)
                            {

                            }

                        }
                        string nextDate = "01." + startDateMES.AddMonths(1).Month.ToString() + "." + startDateMES.AddMonths(1).Year.ToString();
                        startDateMES = Convert.ToDateTime(nextDate);

                        startTimeMES = new TimeSpan();
                    }
                }
                catch (Exception ex)
                {

                    Scope.WriteError("BSU/WriteDataToMesReport" + ex.Message);
                }


                isRunning = false;
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("BSUErrorsToMES - " + ex.Message);
            }
            
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
                TimerCallback callback = new TimerCallback(WriteDataToMesReport);
                timer = new System.Threading.Timer(callback, null, 0, 60000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("BSUErrorsToMES - " + ex.Message);
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
                lastEntry.Message = msg;
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
    }
}



        
  

