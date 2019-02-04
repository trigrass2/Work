using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;


namespace DataCollector
{
    public class AwmRepToSql
    {
        private static List<string> paths = new List<string>() { "\\\\10.5.0.181\\1\\AwmApp\\Dta\\Cam\\rep\\", "\\\\10.5.0.178\\1\\AwmApp\\Dta\\Cam\\rep\\" };
        private int awm = paths.Count;
        private AWM_dbEntities _dbContext;
        private Timer timer;

        public ObservableCollection<Rep> Reps { get; set; }

        public AwmRepToSql()
        {
            _dbContext = new AWM_dbEntities();
            Reps = new ObservableCollection<Rep>();
        }

        public void StartModule(object sender, DoWorkEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Rep заполняется...");
            if (_dbContext.Rep.Count() == 0)
            {
                for (int i = awm; i > 0; i--)
                {
                    var needFile = Directory.GetFiles(paths[i - 1], "*.csv");

                    foreach (var f in needFile)
                    {
                        SaveToDB(f, i, Convert.ToDateTime("1900-01-01 00:00:00"));
                        SaveData(Reps, _dbContext);                        
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
                    case 1:
                        start = _dbContext.Rep.Where(x => x.Rep_id == 1)
                                          .Max(x => x.Timestamp); break;

                    case 2:
                        start = _dbContext.Rep.Where(x => x.Rep_id == 2)
                                          .Max(x => x.Timestamp); break;
                }

                var needFile = Directory.GetFiles(paths[i - 1], "*.csv").Where(x => File.GetLastWriteTime(x) >= start);

                DateTime newFileDateTime = File.GetLastWriteTime(needFile.ElementAt(needFile.Count() - 1));

                if (newFileDateTime >= start)
                {
                    foreach (var f in needFile)
                    {
                        SaveToDB(f, i, start);
                        if(Reps.Count != 0)
                        {
                            SaveData(Reps, _dbContext);
                        }
                       
                    }
                }
            }
        }

        private void SaveData(ObservableCollection<Rep> insertData, AWM_dbEntities context)
        {            
            _dbContext.Rep.AddRange(insertData);
            _dbContext.SaveChanges();
            Reps.Clear();         
        }

        private void SaveToDB(string path, int repIndex, DateTime lastWriteInDB)
        {
            string[] str = {";"};
            string[] spl = { "\r\n" };
            List<string> listCsv = new List<string>();
            Rep oneRep = new Rep();

            using (StreamReader rd = new StreamReader(new FileStream(path, FileMode.Open), Encoding.Default))
            {
                listCsv.AddRange(rd.ReadToEnd().Split(str, StringSplitOptions.RemoveEmptyEntries));
            }          

            while(listCsv.Count > 1)
            {                
                oneRep.Timestamp = Convert.ToDateTime(GetDate((listCsv[0]), listCsv[1]));
                if(oneRep.Timestamp <= lastWriteInDB)
                {                     
                    listCsv.RemoveAt(0);
                    if (listCsv.Contains(listCsv.Find(x => x.Contains("\r\n"))))
                    {
                        listCsv.RemoveRange(0, listCsv.FindIndex(x => x.Contains("\r\n")));
                    }
                    else { break; }
                                        
                    continue;
                }
                else
                {
                    listCsv.RemoveAt(listCsv.FindIndex(x => x.Length >= 8));
                    listCsv.RemoveAt(listCsv.FindIndex(x => x.Length == 6));
                }
                
                oneRep.Status = listCsv.Find(x => x.Length > 1);
                listCsv.RemoveAt(listCsv.FindIndex(x => x.Length > 1));
                oneRep.Mesh_id = Convert.ToInt32(listCsv[0]);                
                listCsv.RemoveRange(listCsv.FindIndex(x => x == oneRep.Mesh_id.ToString()), listCsv.FindIndex(x => x.Length > 4));
                oneRep.Mesh_name = (listCsv.Find(x => x.Length > 4));
                listCsv.RemoveRange(listCsv.FindIndex(x => x.Length > 4), listCsv.FindIndex(x => x.Contains("\r\n")));
                oneRep.Rep_id = repIndex;
                Reps.Add(new Rep() { Mesh_id = oneRep.Mesh_id, Mesh_name = oneRep.Mesh_name, Rep_id = oneRep.Rep_id, Status = oneRep.Status, Timestamp = oneRep.Timestamp});
               
            }            
        }

        private string GetDate(string strDate, string strTime)
        {
            string[] tempArr = new string[2];
            string[] splitter = { "\r\n" };

            string strFullDate = String.Empty;

            if(strTime.Length < 6)
            {
                while (strTime.Length < 6)
                {
                    strTime = strTime.Insert(0, "0");
                }
            }

            if(strDate.Length > 8)
            {
                tempArr = strDate.Split(splitter, StringSplitOptions.RemoveEmptyEntries);                
                
                if(tempArr.Length > 1)
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
    }
}

