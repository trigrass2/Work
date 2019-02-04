using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace DataAllocator
{
    public class AwmHmiCraneToSql
    {
        private static object locker = new object();
        private static List<string> paths = new List<string>() { "\\\\10.5.0.130\\1\\AwmApp\\Dta\\HmiCrane\\Log\\", "\\\\10.5.0.178\\1\\AwmApp\\Dta\\HmiCrane\\Log\\", "\\\\10.5.0.181\\1\\AwmApp\\Dta\\HmiCrane\\Log\\" };
        private LOG_ZAVOD_NFEntities _dbContext;
        private System.Threading.Timer timer;
        private static bool isRunning;
        public LastEntry lastEntryOne;
        public LastEntry lastEntryTwo;
        public LastEntry lastEntryThree;
        private string messageLog1 = string.Empty;
        private string messageLog2 = string.Empty;
        private string messageLog3 = string.Empty;
        public LastEntry stateModule = new LastEntry() { Message = "выключен" };

        private delegate void SenderText(string lastEntry, Form form);

        private readonly SenderText senderText;
        private readonly SenderText senderState;
        
        public ObservableCollection<AWM_HmiCrane_Log> Logs { get; set; }

        public AwmHmiCraneToSql()
        {            
            try
            {
                _dbContext = new LOG_ZAVOD_NFEntities();
                Logs = new ObservableCollection<AWM_HmiCrane_Log>();
                if (_dbContext.AWM_HmiCrane_Log != null && _dbContext.AWM_HmiCrane_Log.Count() > 0)
                {

                    messageLog1 = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 1) ? "AWM_HmiCrane_Log(1). Время: " + _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 1).Max(x => x.Timestamp) : new DateTime().ToString();
                    messageLog2 = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 2) ? "AWM_HmiCrane_Log(2). Время: " + _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 2).Max(x => x.Timestamp) : new DateTime().ToString();
                    messageLog3 = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 3) ? "AWM_HmiCrane_Log(3). Время: " + _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 3).Max(x => x.Timestamp) : new DateTime().ToString();
                }
            }
            catch (Exception ex)
            {
                Scope.WriteError("public AwmHmiCraneToSql().. " + ex.Message);
            }

            lastEntryOne = new LastEntry() { Message = messageLog1 };
            lastEntryTwo = new LastEntry() { Message = messageLog2 };
            lastEntryThree = new LastEntry() { Message = messageLog3 };
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
                if (!_dbContext.AWM_HmiCrane_Log.Any())
                {
                    for (int i = paths.Count; i > 0; i--)
                    {
                        var needFile = Directory.GetFiles(paths[i - 1], "*.log");

                        foreach (var f in needFile)
                        {
                            SaveToDB(f, i, new DateTime());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Scope.WriteError(" AwmHmiCraneToSql/StartModule" + ex.Message);
            }

            try
            {
                TimerCallback callback = new TimerCallback(UpdateDB);
                timer = new System.Threading.Timer(callback, null, 0, 60000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("AwmHmiCraneToSql - " + ex.Message);
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
                    _dbContext = new LOG_ZAVOD_NFEntities();
                    try
                    {
                        switch (i)
                        {
                            case 1:
                                start = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 1) ? _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 1)
                                                                                                                       .Max(x => x.Timestamp) : defaultTime; break;

                            case 2:
                                start = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 2) ? _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 2)
                                                                                                                        .Max(x => x.Timestamp) : defaultTime; break;

                            case 3:
                                start = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 3) ? _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 3)
                                                                                                                                 .Max(x => x.Timestamp) : defaultTime; break;


                        }
                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError(" Поиск в _dbContext.AWM_HmiCrane_Log -> " + ex.Message);
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
                    try
                    {
                        needFile = new List<string>(Directory.GetFiles(paths[i - 1], "*.log").Where(x => File.GetLastWriteTime(x) >= start));
                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError("AwmHmiCrane/Поиск файлов needFile = new List<string>(Directory.GetFiles(paths[i - 1],  *.log).Where(x => File.GetLastWriteTime(x) >=  start)) -> " + ex.Message);
                    }

                    DateTime newFileDateTime;

                    if (needFile.Count() != 0)
                    {
                        newFileDateTime = File.GetLastWriteTime(needFile.ElementAt(needFile.Count() - 1));
                        if (newFileDateTime >= start)
                        {
                            foreach (var f in needFile)
                            {
                                SaveToDB(f, i, start);
                            }
                        }
                    }
                }
                isRunning = false;
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("AwmHmiCraneToSql - " + ex.Message);
            }
            
        }
        //save
        private void SaveData(ObservableCollection<AWM_HmiCrane_Log> insertData, int index)
        {
            if (insertData != null && insertData.Any())
            {
                try
                {
                    LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                    Scope.BulkInsert(insertData.ToList(), new SqlConnection(_context.Database.Connection.ConnectionString), "AWM_HmiCrane_Log");

                    switch (index)
                    {
                        case 1: messageLog1 = "AWM_HmiCrane_Log(1). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                        case 2: messageLog2 = "AWM_HmiCrane_Log(2). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                        case 3: messageLog3 = "AWM_HmiCrane_Log(3). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                    }

                }
                catch (Exception ex)
                {
                    Scope.WriteError(" SaveChange in AwmHmiCraneToSql -> " + ex.Message);
                }

                try
                {
                    SendTextOnActiveForm(messageLog1, Form.ActiveForm);
                    SendTextOnActiveForm(messageLog2, Form.ActiveForm);
                    SendTextOnActiveForm(messageLog3, Form.ActiveForm);
                }
                catch (Exception) { }

            }

            Logs.Clear();
        }

        private void SaveToDB(string path, int logId, DateTime lastWriteInDB)
        {
            if (logId == 2)
            {
                try
                {
                    lastWriteInDB = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 2) ? _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 2)
                                                                                                                   .Max(x => x.Timestamp) : new DateTime();
                }
                catch (Exception ex)
                {
                    Scope.WriteError("AwmHmiCrane/SaveToDB/lastWriteInDB = _dbContext.AWM_HmiCrane_Log.Any(x => x.Log_id == 2) ? _dbContext.AWM_HmiCrane_Log.Where(x => x.Log_id == 2) -> " + ex.Message);
                }

            }

            List<string> lines = new List<string>();
            try
            {
                lines = File.ReadAllLines(path, Encoding.Default).ToList();
                if (!lines.Any()) return;
            }
            catch (Exception ex)
            {

                Scope.WriteError("AwmHmiCrane/SaveToDB/lines = File.ReadAllLines(path, Encoding.Default).ToList() -> " + ex.Message);
            }

            AWM_HmiCrane_Log lastItem = new AWM_HmiCrane_Log();
            AWM_HmiCrane_Log needItem = new AWM_HmiCrane_Log();

            try
            {
                lastItem = _dbContext.AWM_HmiCrane_Log.Where(x => x.Timestamp == lastWriteInDB && x.Log_id == logId).First();
            }
            catch (Exception ex)
            {
                Scope.WriteError("AwmHmiCrane/lastItem = _dbContext.AWM_HmiCrane_Log.Where(x => x.Timestamp == lastWriteInDB && x.Log_id == logId).First(); " + ex.Message);
            }

            List<string> listCsv = new List<string>();
            string[] str = { ";" };
            string lastTime = "#" + lastWriteInDB.ToString("dd/MM/yyyy HH:mm:ss").Replace(".", "/") + "#";
            int k = lines.FindIndex(x => x.Contains(lastTime)) + 1;


            for (int i = k; i < lines.Count; i++)
            {
                listCsv = lines[i].Split(str, StringSplitOptions.RemoveEmptyEntries).ToList();
                try
                {
                    if (!listCsv.Any() || !DateTime.TryParse(listCsv[0], out DateTime d) || GetDate(listCsv[0]) == lastItem.Timestamp && Convert.ToInt32(listCsv[2]) == lastItem.Error_id)
                    {
                        continue;
                    }
                    
                    needItem.Timestamp = GetDate(listCsv[0]);
                    needItem.Eventtype = Convert.ToInt32(listCsv[1]);
                    needItem.Error_id = Convert.ToInt32(Double.Parse(listCsv[2].Replace(".",",")));
                    needItem.Status = Convert.ToInt32(listCsv[3]);
                    needItem.Desk_en = listCsv.Any(x => Regex.IsMatch(x, "[A-Z]")) ? listCsv.Find(x => Regex.IsMatch(x, "[A-Z]")) : string.Empty;
                    needItem.Desk_ru = listCsv.Any(x => Regex.IsMatch(x, "[А-Я]")) ? listCsv.Find(x => Regex.IsMatch(x, "[А-Я]")) : string.Empty;

                    Logs.Add(new AWM_HmiCrane_Log() { Timestamp = needItem.Timestamp, Eventtype = needItem.Eventtype, Error_id = needItem.Error_id, Status = needItem.Status, Desk_en = needItem.Desk_en, Desk_ru = needItem.Desk_ru, Log_id = logId });
                    
                }
                catch (Exception ex)
                {
                    Scope.WriteError("AwmHmiCrane/for (int i = k; i < lines.Count; i++) -> " + ex.Message);
                }

            }
            SaveData(Logs, logId);
        }

        private DateTime GetDate(string str)
        {
            DateTime result = new DateTime();
            try
            {
                result = Convert.ToDateTime(str.Replace("#", "").Replace(".", ":"));
            }
            catch (Exception ex)
            {
                Scope.WriteError("AwmHmiCrane/GetDate -> " + ex.Message);
            }
            return result;
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
                if (msg.Contains("AWM_HmiCrane_Log(1)"))
                {
                    lastEntryOne.Message = msg;
                }
                else if (msg.Contains("AWM_HmiCrane_Log(2)"))
                {
                    lastEntryTwo.Message = msg;
                }
                else if (msg.Contains("AWM_HmiCrane_Log(3)"))
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
