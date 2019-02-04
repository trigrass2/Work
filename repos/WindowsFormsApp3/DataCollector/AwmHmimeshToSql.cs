using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace DataCollector
{
    public class AwmHmimeshToSql
    {
        private static List<string> paths = new List<string>() { "\\\\10.5.0.181\\1\\AwmApp\\Dta\\HmiMesh\\Log\\", "\\\\10.5.0.178\\1\\AwmApp\\Dta\\HmiMesh\\Log\\" };
        private int awm = paths.Count;
        private AWM_dbEntities _dbContext;
        private Timer timer;

        public ObservableCollection<Hmimesh_Log> Logs { get; set; }

        public AwmHmimeshToSql()
        {
            _dbContext = new AWM_dbEntities();
            Logs = new ObservableCollection<Hmimesh_Log>();                     
        }

        public void StartModule(object sender, DoWorkEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Log заполняется...");
            if (_dbContext.Hmimesh_Log.Count() == 0)
            {
                for (int i = awm; i > 0; i--)
                {
                    var needFile = Directory.GetFiles(paths[i - 1], "*.log");

                    foreach (var f in needFile)
                    {
                        SaveToDB(f, i, Convert.ToDateTime("1900-01-01 00:00:00"));
                        SaveData(Logs, _dbContext);                        
                    }
                }
            }

            TimerCallback callback = new TimerCallback(UpdateDB);
            timer = new Timer(callback, null, 0, 60000000);

        }

        public void StopModule()
        {
            if(timer != null)
            {
                timer.Change(Timeout.Infinite, 0);
                timer.Dispose();
            }
            
        }

        public void UpdateDB(object obj)
        {
            
            DateTime start = new DateTime();

            for (int i = awm; i > 0; i--)
            {
                switch (i)
                {
                    case 1:
                        start = _dbContext.Hmimesh_Log.Where(x => x.Log_id == 1)
                                          .Max(x => x.Timestamp); break;

                    case 2:
                        start = _dbContext.Hmimesh_Log.Where(x => x.Log_id == 2)
                                          .Max(x => x.Timestamp); break;
                }

                var needFile = Directory.GetFiles(paths[i - 1], "*.log").Where(x => File.GetLastWriteTime(x) >= start);

                DateTime newFileDateTime = File.GetLastWriteTime(needFile.ElementAt(needFile.Count() - 1));

                if (newFileDateTime > start)
                {

                    foreach (var f in needFile)
                    {

                       SaveToDB(f, i, start);
                        if(Logs.Count != 0)
                        {
                            SaveData(Logs, _dbContext);
                        }                                              
                                            
                    }
                }
            }
        }

        private void SaveData(ObservableCollection<Hmimesh_Log> insertData, AWM_dbEntities context)
        {
            _dbContext.Hmimesh_Log.AddRange(insertData);
            _dbContext.SaveChanges();
            Logs.Clear(); 
        }

        private void SaveToDB(string path, int logIndex, DateTime lastWriteInDB)
        {
            string[] str = { ";", "\r\n", "\0", "#" };

            using (StreamReader rd = new StreamReader(new FileStream(path, FileMode.Open), Encoding.Default))
            {
                str = rd.ReadToEnd().Split(str, StringSplitOptions.None);
            }

            string[] strTemp = str;
            DateTime dt = new DateTime();

            for (int i = 0; i < str.Length-1; i = i + 17)
            {         

                dt = Convert.ToDateTime(GetDate(str[i + 1]));

                if (dt > lastWriteInDB)
                {
                    Logs.Add(new Hmimesh_Log() { Timestamp = dt, Eventtype = Convert.ToInt32(str[i + 3]), Error_id = Convert.ToInt32(str[i + 4].Replace(".", "")), Status = Convert.ToInt32(str[i + 5]), Desk_en = str[i + 6], Desk_ru = str[i + 12], Log_id = logIndex });
                }               

            }
        }

        private string GetDate(string str)
        { 
            str = str.Replace(".", ":");
            return str;
        }
    }
}
