using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Threading;

namespace UsedSchalerParserLogic
{
    public class LogParser
    {
        private static readonly string PU1Directoty = ConfigurationManager.ConnectionStrings["Pu1ConnectionString"].ConnectionString;
        private static readonly string PU2Directoty = ConfigurationManager.ConnectionStrings["Pu2ConnectionString"].ConnectionString;
        private static readonly string PU3Directoty = ConfigurationManager.ConnectionStrings["Pu3ConnectionString"].ConnectionString;

        private readonly LOG_ZAVOD_NFEntities _dbContext = new LOG_ZAVOD_NFEntities();
        private List<Used_schaler> Used_Schalers = new List<Used_schaler>();        

        private Timer timer;
        private bool isRunning;

        public Logger logger = new Logger();

        public void Start()
        {

            try
            {
                logger.Write("OnStart");
                TimerCallback callback = new TimerCallback(StartMetods);
                timer = new Timer(callback, null, 0, 36000000);//36000000 - 10 часов
            }
            catch (Exception ex)
            {
                logger.Write($"Start -> {ex.Message}");
            }
        }

        public void Stop()
        {            
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, 0);
                timer.Dispose();
                logger.Write("OnStop");
            }
            logger.Dispose();
        } 

        private void StartMetods(object obj)
        {
            if (isRunning) return;
            isRunning = true;

            SendData(PU1Directoty, "PU1");
            SendData(PU2Directoty, "PU2");
            SendData(PU3Directoty, "PU3");

            isRunning = false;
        }

        private void SendData(string file, string puId)
        {
            try
            {
                Used_Schalers.Clear();
                int d = 0;
                Used_schaler lastItem = new Used_schaler();

                List<string> listFiles = new List<string>(File.ReadAllLines(file));

                if (_dbContext.Used_schaler.Any(x => x.PU_id == puId))
                {
                    for (int i = listFiles.Count-1; i > 0; i--)
                    {
                        var item = GetModel(listFiles[i], puId);
                        lastItem = _dbContext.Used_schaler.Where(x => x.Date == item.Date && x.ProdNr == item.ProdNr && x.Unknow == item.Unknow && x.TypeFormwork == item.TypeFormwork && x.NameFormwork == item.NameFormwork).FirstOrDefault();
                        if (lastItem != null) break;
                    }

                    var dd = Convert.ToDateTime(lastItem.Date).ToString("dd.MM.yyyy HH:mm:ss");
                    string checkString = $"{dd}\t{lastItem.UserName}\t ProdNr:\t{lastItem.ProdNr}\t{lastItem.Unknow}\tSchalerKennung:\t{lastItem.TypeFormwork}\tName:\t{lastItem.NameFormwork}";

                    var coutLastInDB = _dbContext.Used_schaler.Where(x => x.Date == lastItem.Date && x.ProdNr == lastItem.ProdNr && x.TypeFormwork == lastItem.TypeFormwork && x.NameFormwork == lastItem.NameFormwork).Count();
                    int countLastInFile = listFiles.Where(x => x.Contains(checkString)).Count();
                    if (coutLastInDB < countLastInFile)
                    {
                        d = listFiles.FindLastIndex(x => x.Contains(checkString)) - (countLastInFile - coutLastInDB);
                    }
                    else if (coutLastInDB == countLastInFile)
                    {
                        d = listFiles.FindLastIndex(x => x.Contains(checkString)) + 1;
                    }
                }             
                 
                for(int i = d; i < listFiles.Count; i++)
                {
                    Used_Schalers.Add(GetModel(listFiles[i], puId));
                }

                if (Used_Schalers.Count > 10000)
                {
                    BulkInsert(Used_Schalers, new SqlConnection(_dbContext.Database.Connection.ConnectionString), "Used_schaler");
                    logger.Write($"{DateTime.Now} Запись в БД успешно выполнена");
                }
                else if(Used_Schalers.Count > 0)
                {
                    _dbContext.Used_schaler.AddRange(Used_Schalers);
                    _dbContext.SaveChanges();
                    logger.Write($"{DateTime.Now} {file} Запись в БД успешно выполнена");
                }

            }
            catch (Exception ex)
            {               
                logger.Write($"SendData -> {ex.Message}");
            }            
            
        }

        private Used_schaler GetModel(string lineFileLog, string puId)
        {
            try
            {
                
                var splitstr = lineFileLog.Split(new string[] {"\t"},StringSplitOptions.RemoveEmptyEntries);
                
                Used_schaler schaler = new Used_schaler();
                if (splitstr.Length == 11)
                {
                    schaler = new Used_schaler
                    {
                        PU_id = puId.ToString(),
                        Date = Convert.ToDateTime(splitstr[2]),
                        UserName = splitstr[3],
                        ProdNr = int.Parse(splitstr[5]),
                        Unknow = splitstr[6],
                        TypeFormwork = int.Parse(splitstr[8]),
                        NameFormwork = splitstr[10]
                    };
                }
                if (splitstr.Length == 10)
                {
                    schaler = new Used_schaler
                    {
                        PU_id = puId.ToString(),
                        Date = Convert.ToDateTime(splitstr[2]),
                        UserName = null,
                        ProdNr = int.Parse(splitstr[4]),
                        Unknow = splitstr[5],
                        TypeFormwork = int.Parse(splitstr[7]),
                        NameFormwork = splitstr[9]
                    };
                }

                return schaler;
            }
            catch (Exception ex)
            {
                logger.Write($"GetModel -> {ex.Message}");
                return null;
            }
        }

        private static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                DataColumn dataColumn = new DataColumn();
                dataColumn.AllowDBNull = true;
                dataColumn.ColumnName = prop.Name;
                dataColumn.DataType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(dataColumn);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        private static void BulkInsert<T>(IList<T> data, SqlConnection connection, string tablename)
        {
            DataTable dataTable = ToDataTable(data);
            SqlTransaction transaction = null;
            connection.Open();
            try
            {
                transaction = connection.BeginTransaction();
                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = tablename;
                    foreach (DataColumn dc in dataTable.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    sqlBulkCopy.WriteToServer(dataTable);
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }
    }
}
