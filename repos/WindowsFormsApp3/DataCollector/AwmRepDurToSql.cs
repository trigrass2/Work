using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace DataCollector
{
    public class AwmRepDurToSql
    {
        private static List<string> paths = new List<string>() { "\\\\10.5.0.181\\1\\AwmApp\\Dta\\Cam\\rep\\dur", "\\\\10.5.0.178\\1\\AwmApp\\Dta\\Cam\\rep\\dur" };
        private int awm = paths.Count;
        private AWM_dbEntities _dbContext;
        private Timer timer;

        public ObservableCollection<Dur> Durs { get; set; }

        public AwmRepDurToSql()
        {
            _dbContext = new AWM_dbEntities();            
            Durs = new ObservableCollection<Dur>();         
                    
        }

        public void StartModule(object sender, DoWorkEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Dur заполняется...");
            if (_dbContext.Dur.Count() == 0)
            {
                for (int i = awm; i > 0; i--)
                {
                    var needFile = Directory.GetFiles(paths[i - 1], "*.csv");

                    foreach (var f in needFile)
                    {
                        SaveToDB(f, i, Convert.ToDateTime("1900-01-01 00:00:00"));
                        SaveData(Durs, _dbContext);                        
                    }
                }
            }

            TimerCallback callback = new TimerCallback(UpdateDB);
            timer = new Timer(callback, null, 0, 60000000);

        }

        public void StopModule()
        {
            if (timer != null)
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
                    case 1: start = _dbContext.Dur.Where(x => x.Dur_id == 1)
                                              .Max(x => x.Timestamp); break;

                    case 2: start = _dbContext.Dur.Where(x => x.Dur_id == 2)
                                              .Max(x => x.Timestamp); break;
                }

                var needFile = Directory.GetFiles(paths[i - 1], "*.csv").Where(x => File.GetLastWriteTime(x) >= start);

                DateTime newFileDateTime;
                if (needFile.Count() != 0)
                {
                    newFileDateTime = File.GetLastWriteTime(needFile.ElementAt(needFile.Count() - 1));
                    if (newFileDateTime >= start)
                    {
                        foreach (var f in needFile)
                        {
                            SaveToDB(f, i, start);
                            if(Durs.Count != 0)
                            {
                                SaveData(Durs, _dbContext);
                            }

                        }
                    }
                }
                                

                
            }           
        }
        
        private void SaveData(ObservableCollection<Dur> insertData, AWM_dbEntities context)
        {
            _dbContext.Dur.AddRange(insertData);
            _dbContext.SaveChanges();
            Durs.Clear();
        }

        private void SaveToDB(string path, int durIndex, DateTime lastWriteInDB)
        {
            string[] str = { ";", "\r\n","\0" };

            using (StreamReader rd = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                str = rd.ReadToEnd().Split(str, StringSplitOptions.RemoveEmptyEntries);
            }

            string[] strTemp = str;
            string date = GetDate(Path.GetFileNameWithoutExtension(path));

            for (int i = 0; i < str.Length; i = i + 2)
            {                
                DateTime strDate = Convert.ToDateTime(date + " " + strTemp[i]);

                if(strDate > lastWriteInDB)
                {
                    Durs.Add(new Dur() { Timestamp = strDate, Seconds = Convert.ToInt32(str[i + 1]), Dur_id = durIndex });
                }
                
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
    }
}
