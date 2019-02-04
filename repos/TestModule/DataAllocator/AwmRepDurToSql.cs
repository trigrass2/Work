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
    public class AwmRepDurToSql
    {
        private static object locker = new object();
        private static List<string> paths = new List<string>() { "\\\\10.5.0.130\\1\\AwmApp\\Dta\\Cam\\rep\\dur\\", "\\\\10.5.0.178\\1\\AwmApp\\Dta\\Cam\\rep\\dur", "\\\\10.5.0.181\\1\\AwmApp\\Dta\\Cam\\rep\\dur" };
        private readonly LOG_ZAVOD_NFEntities _dbContext;        
        private System.Threading.Timer timer;
        private static bool isRunning;
        public string messageDur1 = string.Empty;
        public string messageDur2 = string.Empty;
        public string messageDur3 = string.Empty;
        public LastEntry lastEntryOne;
        public LastEntry lastEntryTwo;
        public LastEntry lastEntryThree;
        public LastEntry stateModule = new LastEntry() { Message = "выключен" };
        
        private delegate void SenderText(string lastEntry, Form form);

        private readonly SenderText senderText;
        private readonly SenderText senderState;        

        public ObservableCollection<AWM_Dur> Durs { get; set; }

        public AwmRepDurToSql()
        {
            try
            {
                _dbContext = new LOG_ZAVOD_NFEntities();
                Durs = new ObservableCollection<AWM_Dur>();

                if (_dbContext.AWM_Dur.Any())
                {
                    messageDur1 = _dbContext.AWM_Dur.Any(x => x.Dur_id == 1) ? "AWM_Dur(1). Время: " + _dbContext.AWM_Dur.Where(x => x.Dur_id == 1).Max(x => x.Timestamp) : new DateTime().ToString();
                    messageDur2 = _dbContext.AWM_Dur.Any(x => x.Dur_id == 2) ? "AWM_Dur(2). Время: " + _dbContext.AWM_Dur.Where(x => x.Dur_id == 2).Max(x => x.Timestamp) : new DateTime().ToString();
                    messageDur3 = _dbContext.AWM_Dur.Any(x => x.Dur_id == 3) ? "AWM_Dur(3). Время: " + _dbContext.AWM_Dur.Where(x => x.Dur_id == 3).Max(x => x.Timestamp) : new DateTime().ToString();
                }
            }
            catch (Exception ex)
            {
                Scope.WriteError("messageDur1, messageDur2, messageDur3" + ex.Message);
            }              
                       
            lastEntryOne = new LastEntry() { Message = messageDur1 };
            lastEntryTwo = new LastEntry() { Message = messageDur2 };
            lastEntryThree = new LastEntry() { Message = messageDur3 };
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
                if (!_dbContext.AWM_Dur.Any())
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
                Scope.WriteError(" AwmRepDurToSql/StartModule -> " + ex.Message);
            }

            try
            {
                TimerCallback callback = new TimerCallback(UpdateDB);
                timer = new System.Threading.Timer(callback, null, 0, 60000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("AwmRepDurToSql - " + ex.Message);
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
                                start = _dbContext.AWM_Dur.Any(x => x.Dur_id == 1) ? _dbContext.AWM_Dur.Where(x => x.Dur_id == 1)
                                                                                                       .Max(x => x.Timestamp) : defaultTime; break;

                            case 2:
                                start = _dbContext.AWM_Dur.Any(x => x.Dur_id == 2) ? _dbContext.AWM_Dur.Where(x => x.Dur_id == 2)
                                                                                                       .Max(x => x.Timestamp) : defaultTime; break;

                            case 3:
                                start = _dbContext.AWM_Dur.Any(x => x.Dur_id == 3) ? _dbContext.AWM_Dur.Where(x => x.Dur_id == 3)
                                                                                                                 .Max(x => x.Timestamp) : defaultTime; break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Scope.WriteError(" Поиск в _dbContext.AWM_Dur -> " + ex.Message);
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
                        needFile = new List<string>(Directory.GetFiles(paths[i - 1], "*.csv").Where(x => File.GetLastWriteTime(x) >= start));
                    }
                    catch (Exception ex)
                    {

                        Scope.WriteError("AwmRepDur/UpdateDB/needFile = new List<string>(Directory.GetFiles(paths[i - 1],  *.csv).Where(x => File.GetLastWriteTime(x) >= start)) -> " + ex.Message);
                    }


                    DateTime newFileDateTime;
                    if (needFile.Any())
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
                Scope.WriteGlobalError("AwmRepDurToSql " + ex.Message);
            }
            
        }

        private void SaveData(ObservableCollection<AWM_Dur> insertData , int index)
        {
            try
            {
                LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                Scope.BulkInsert(insertData, new SqlConnection(_context.Database.Connection.ConnectionString), "AWM_Dur");

                switch (index)
                {
                    case 1: messageDur1 = "AWM_Dur(1). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                    case 2: messageDur2 = "AWM_Dur(2). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                    case 3: messageDur3 = "AWM_Dur(3). Время: " + Convert.ToString(insertData.Max(x => x.Timestamp)); break;
                }

            }
            catch(Exception ex)
            {
                Scope.WriteError(" SaveChange in AwmRepDurTOSql -> " + ex.Message);
            }

            try
            {
                SendTextOnActiveForm(messageDur1, Form.ActiveForm);
                SendTextOnActiveForm(messageDur2, Form.ActiveForm);
                SendTextOnActiveForm(messageDur3, Form.ActiveForm);
            }
            catch (Exception)
            {
             
            }

            Durs.Clear();
        }

        private void SaveToDB(string path, int durIndex, DateTime lastWriteInDB)
        {
            if (durIndex == 2)
            {
                lastWriteInDB = _dbContext.AWM_Dur.Any(x => x.Dur_id == 2) ? _dbContext.AWM_Dur.Where(x => x.Dur_id == 2)
                                                                                                   .Max(x => x.Timestamp) : new DateTime();
            }

            string[] str = { ";", "\r\n", "\0" };

            try
            {
                using (StreamReader rd = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    str = rd.ReadToEnd().Split(str, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch(Exception ex)
            {
                Scope.WriteError("AwmRepDur/ " + ex.Message);
                return;
            }
                      

            List<string> listStr = new List<string>();
            listStr = str.ToList();
            
            string date = GetDate(Path.GetFileNameWithoutExtension(path));            


            for (int i = 0; i < listStr.Count; i = i + 2)
            {
                DateTime strDate = Convert.ToDateTime(date + " " + listStr[i]);
                try
                {
                    if (strDate.ToShortDateString() == lastWriteInDB.ToShortDateString() && strDate.TimeOfDay < lastWriteInDB.TimeOfDay)
                    {
                        listStr.RemoveRange(0, listStr.FindIndex(x => x == lastWriteInDB.ToString("HH:mm:ss")) + 2);
                        if (listStr.Count == 0) break;
                        strDate = Convert.ToDateTime(date + " " + listStr[0]);
                    }
                }
                catch(FormatException ex)
                {
                    Scope.WriteError("AwmRepDurToSql/SaveToDB/for (int i = 0; i < listStr.Count; i = i + 2) -> " + ex.Message);
                }
                
                

                try
                {
                    if (strDate > lastWriteInDB)
                    {
                        Durs.Add(new AWM_Dur() { Timestamp = strDate, Seconds = Convert.ToInt32(listStr[i + 1]), Dur_id = durIndex });                                                  
                    }
                }
                catch(Exception ex)
                {
                    Scope.WriteError(" Durs.Add(new Dur()... -> " + ex.Message);
                }              

            }
            if(Durs != null && Durs.Any())
            {
                SaveData(Durs, durIndex);
            }
                
          
        }

        private string GetDate(string str)
        {
            char[] a = str.ToArray();
            string[] b = new string[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                b[i] = a[i].ToString();
            }


            b[3] += "-";
            b[5] += "-";

            str = String.Empty;

            for (int i = 0; i < b.Length; i++)
            {
                str += b[i];
            }

            return str;
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
                if(msg.Contains("AWM_Dur(1)"))
                {
                    lastEntryOne.Message = msg;
                }else if(msg.Contains("AWM_Dur(2)"))
                      {
                         lastEntryTwo.Message = msg;

                      }else if(msg.Contains("AWM_Dur(3)"))
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