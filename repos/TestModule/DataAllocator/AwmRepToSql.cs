using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace DataAllocator
{
    public class AwmRepToSql
    {
        private static object locker = new object();
        private static List<string> paths = new List<string>() { "\\\\10.5.0.130\\1\\AwmApp\\Dta\\Cam\\rep\\", "\\\\10.5.0.178\\1\\AwmApp\\Dta\\Cam\\rep\\", "\\\\10.5.0.181\\1\\AwmApp\\Dta\\Cam\\rep\\"};
        private readonly LOG_ZAVOD_NFEntities _dbContext;
        private System.Threading.Timer timer;

        public ObservableCollection<AWM_Rep> Reps { get; set; }
        private static bool isRunning;
        private string messageRep1 = string.Empty;
        private string messageRep2 = string.Empty;
        private string messageRep3 = string.Empty;

        public LastEntry lastEntryOne;
        public LastEntry lastEntryTwo;
        public LastEntry lastEntryThree;

        private delegate void SenderText(string lastEntry, Form form);
        SenderText senderText;
        SenderText senderState;
        public LastEntry stateModule = new LastEntry() { Message = "выключен" };

        public AwmRepToSql()
        {
            try
            {
                _dbContext = new LOG_ZAVOD_NFEntities();
                Reps = new ObservableCollection<AWM_Rep>();
                if (_dbContext.AWM_Rep.Any())
                {
                    messageRep1 = _dbContext.AWM_Rep.Any(x => x.Rep_id == 1) ? "AWM_Rep(1). Время: " + _dbContext.AWM_Rep.Where(x => x.Rep_id == 1).Max(x => x.Timestamp) : new DateTime().ToString();
                    messageRep2 = _dbContext.AWM_Rep.Any(x => x.Rep_id == 2) ? "AWM_Rep(2). Время: " + _dbContext.AWM_Rep.Where(x => x.Rep_id == 2).Max(x => x.Timestamp) : new DateTime().ToString();
                    messageRep3 = _dbContext.AWM_Rep.Any(x => x.Rep_id == 3) ? "AWM_Rep(3). Время: " + _dbContext.AWM_Rep.Where(x => x.Rep_id == 3).Max(x => x.Timestamp) : new DateTime().ToString();
                }
            }
            catch (Exception ex)
            {
                Scope.WriteError("messageRep1, messageRep2, messageRep3 " + ex.Message);
            }            
            
            lastEntryOne = new LastEntry() { Message = messageRep1 };
            lastEntryTwo = new LastEntry() { Message = messageRep2 };
            lastEntryThree = new LastEntry() { Message = messageRep3 };
            senderText = new SenderText(SendTextOnActiveForm);
            senderState = new SenderText(SendStateOnActiveForm);
        }

        public void StartModule(object sender, DoWorkEventArgs e)
        {
            try
            {
                SendStateOnActiveForm("включен", Form.ActiveForm);
            }
            catch (Exception) { }
            
            try
            {
                if (!_dbContext.AWM_Rep.Any())
                {
                    for (int i = paths.Count; i > 0; i--)
                    {
                        var needFile = Directory.GetFiles(paths[i - 1], "*.csv");

                        foreach (var f in needFile)
                        {
                            SaveToDB(f, i, new DateTime());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Scope.WriteError(" AwmRepToSql/StartModule" + ex.Message);
            }

            try
            {
                TimerCallback callback = new TimerCallback(UpdateDB);
                timer = new System.Threading.Timer(callback, null, 0, 60000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("AwmRepToSql - " + ex.Message);
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

        public void UpdateDB(object obj)
        {
            try
            {
                if (isRunning) return;
                isRunning = true;

                DateTime start = new DateTime();
                DateTime defaultTime = new DateTime();

                for (int i = paths.Count; i > 0; i--)
                {
                    try
                    {
                        switch (i)
                        {
                            case 1:
                                start = _dbContext.AWM_Rep.Any(x => x.Rep_id == 1) ? _dbContext.AWM_Rep.Where(x => x.Rep_id == 1)
                                                                                                   .Max(x => x.Timestamp) : defaultTime; break;

                            case 2:
                                start = _dbContext.AWM_Rep.Any(x => x.Rep_id == 2) ? _dbContext.AWM_Rep.Where(x => x.Rep_id == 2)
                                                                                                       .Max(x => x.Timestamp) : defaultTime; break;

                            case 3:
                                start = _dbContext.AWM_Rep.Any(x => x.Rep_id == 3) ? _dbContext.AWM_Rep.Where(x => x.Rep_id == 3)
                                                                                                                 .Max(x => x.Timestamp) : defaultTime; break;


                        }
                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError(" Поиск в _dbContext.Rep.Where -> " + ex.Message);
                    }

                    if (i == 2)
                    {
                        if (start >= Convert.ToDateTime(start.ToString("dd.MM.yyyy") + " 01:00:00"))
                        {
                            start = start.AddHours(-1);
                        }
                        else
                        {
                            start = start.Add(-(start.TimeOfDay));
                        }
                    }

                    List<string> needFile = new List<string>();
                    DateTime newFileDateTime = new DateTime();

                    try
                    {
                        needFile = new List<string>(Directory.GetFiles(paths[i - 1], "*.csv").Where(x => File.GetLastWriteTime(x) >= start));
                        newFileDateTime = File.GetLastWriteTime(needFile.ElementAt(needFile.Count() - 1));
                    }
                    catch (Exception ex)
                    {

                        Scope.WriteError("AwmRep/needFile = new List<string>(Directory.GetFiles(paths[i - 1], *.csv).Where(x => File.GetLastWriteTime(x) >= start)) -> " + ex.Message);
                    }

                    if (!needFile.Any()) continue;

                    if (newFileDateTime >= start)
                    {
                        foreach (var f in needFile)
                        {
                            SaveToDB(f, i, start);

                        }
                    }
                }
                isRunning = false;
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("AwmRepToSql - " + ex.Message);
            }
            
        }

        private void SaveData(ObservableCollection<AWM_Rep> insertData, int index)
        {
            try
            {
                LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                Scope.BulkInsert(insertData, new SqlConnection(_context.Database.Connection.ConnectionString), "AWM_Rep");
                switch (index)
                {
                    case 1: messageRep1 = "AWM_Rep(1). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp));  break;
                    case 2: messageRep2 = "AWM_Rep(2). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp));  break;
                    case 3: messageRep3 = "AWM_Rep(3). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp));  break;
                }               
                
            }
            catch (Exception ex)
            {
                Scope.WriteError(" SaveChange in AwmRepToSql -> " + ex.Message);
            }

            try
            {
                SendTextOnActiveForm(messageRep1, Form.ActiveForm);
                SendTextOnActiveForm(messageRep2, Form.ActiveForm);
                SendTextOnActiveForm(messageRep3, Form.ActiveForm);
            }
            catch (Exception) { }

            Reps.Clear();
        }

        private void SaveToDB(string path, int repIndex, DateTime lastWriteInDB)
        {            
            if (repIndex == 2)
            {
                try
                {
                    lastWriteInDB = _dbContext.AWM_Rep.Any(x => x.Rep_id == 2) ? _dbContext.AWM_Rep.Where(x => x.Rep_id == 2)
                                                                                              .Max(x => x.Timestamp) : new DateTime();
                }
                catch (Exception ex)
                {

                    Scope.WriteError("AwmRep/SaveToDB/astWriteInDB = _dbContext.AWM_Rep.Any(x => x.Rep_id == 2) ? _dbContext.AWM_Rep.Where(x => x.Rep_id == 2) -> " + ex.Message);
                }
               
            }
            AWM_Rep lastItem = new AWM_Rep();
            try
            {
                lastItem = _dbContext.AWM_Rep.Where(x => x.Timestamp == lastWriteInDB && x.Rep_id == repIndex).First();
            }
            catch (Exception ex)
            {
                Scope.WriteError("AwmHmimesh/lastItem = _dbContext.AWM_Hmimesh_Log.Where(x => x.Timestamp == lastWriteInDB && x.Log_id == logId).First(); " + ex.Message);
            }

            string[] str = { ";" };
            
            List<string> csv = new List<string>();
            List<string> listCsv = new List<string>();
            AWM_Rep oneRep = new AWM_Rep();
            
            try
            {
                
                csv = File.ReadAllLines(path, Encoding.Default).ToList();
                if (!csv.Any()) return;

                string startDate = lastWriteInDB.ToString("yyyy.MM.dd HH:mm:ss").Replace(".", "").Replace(":", "").Replace(" ", ";");
                csv.RemoveRange(0, csv.FindIndex(x => x.Contains(startDate))+1);

                if (!csv.Any()) return;
                for (int i = 0; i < csv.Count; i++)
                {
                    listCsv = csv[i].Split(str, StringSplitOptions.RemoveEmptyEntries).ToList();
                    try
                    {
                        if (Convert.ToDateTime(GetDate(listCsv[0], listCsv[1])) > lastItem.Timestamp || (Convert.ToDateTime(GetDate(listCsv[0], listCsv[1])) == lastItem.Timestamp && listCsv[2] != lastItem.Status))
                        {
                            oneRep.Timestamp = Convert.ToDateTime(GetDate(listCsv[0], listCsv[1]));
                            oneRep.Status = listCsv[2];
                            oneRep.Mesh_id = Convert.ToInt32(listCsv[3]);
                            oneRep.Mesh_name = listCsv[11];

                            Reps.Add(new AWM_Rep { Timestamp = oneRep.Timestamp, Status = oneRep.Status, Mesh_id = oneRep.Mesh_id, Mesh_name = oneRep.Mesh_name, Rep_id = repIndex});
                        }
                    }
                    catch (Exception ex)
                    {

                        Scope.WriteError("AwmRepToSql/for(int i = 0; i < csv.Count; i++) -> " + ex.Message);
                    }   
                    
                }
                if (Reps != null && Reps.Any())
                {
                    SaveData(Reps, repIndex);
                }
            }
            catch (Exception ex)
            {
                Scope.WriteError("AwmRepToSql/SaveToDB/listCsv.AddRange(rd.ReadToEnd().Split... " + ex.Message);
            }            
        }

        private string GetDate(string strDate, string strTime)
        {
            if(strDate.Length < 8)
            {
                return null;
            }

            string[] tempArr = new string[2];
            string[] splitter = { "\r\n" };

            string strFullDate = String.Empty;

            if (strTime.Length < 6)
            {
                while (strTime.Length < 6)
                {
                    strTime = strTime.Insert(0, "0");
                }
            }

            if (strDate.Length > 8)
            {
                tempArr = strDate.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                if (tempArr.Length > 1)
                {
                    strDate = tempArr[1];
                }
                else
                {
                    strDate = tempArr[0];
                }

            }

            char[] a = strDate.ToArray();

            string[] b = new string[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                b[i] = a[i].ToString();
            }

            b[3] += "-";
            b[5] += "-";
            b[7] += " ";

            for (int i = 0; i < b.Length; i++)
            {
                strFullDate += b[i];
            }

            a = strTime.ToArray();
            b = new string[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                b[i] = a[i].ToString();
            }

            b[1] += ":";
            b[3] += ":";

            for (int i = 0; i < b.Length; i++)
            {
                strFullDate += b[i];
            }

            return strFullDate;
        }

        private void SendTextOnActiveForm(string msg, Form form)
        {            
            var activeForm = form;
            if (activeForm.InvokeRequired)
            {
                activeForm.Invoke(senderText, msg, form);
                return;
            }
            if (msg != null)
            {
                if (msg.Contains("AWM_Rep(1)"))
                {
                    lastEntryOne.Message = msg;
                }
                else if (msg.Contains("AWM_Rep(2)"))
                {
                    lastEntryTwo.Message = msg;
                }
                else if (msg.Contains("AWM_Rep(3)"))
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
                return;
            }
            if (state != null)
            {
                stateModule.Message = state;
            }
        }                                                                                                             
    }                                                                                                               
}                                                                                                                   