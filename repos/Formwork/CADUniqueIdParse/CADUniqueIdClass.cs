using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CADUniqueIdParse
{
    public static class CADUniqueIdClass
    {
        private static readonly string fName = "errors.log";

        private static SqlConnection mySqlConnection;
        private static SqlCommand mySqlCommand;
        private static SqlDataAdapter mySqlDataAdapter;
        private static DataTable myDataTable;
        private static readonly string connectionStr = ConfigurationManager.ConnectionStrings["PULeit_Connection_String"].ConnectionString;
        private static DataRow[] cadUniques;
        private static List<CADUniqueIdModel> unnecessaryСadUniqueIdModels = new List<CADUniqueIdModel>();
        private static List<SumCADUnique> sumCADs = new List<SumCADUnique>();        

        private static CADUniqueIdModel GetCADUniq(string rawData)
        {           
            Regex regex;
            MatchCollection matches;
            string match = string.Empty;
            CADUniqueIdModel cadModel;
            
            try
            {   
                
                if(rawData.Any(x => Regex.IsMatch(rawData, @"\d*\.\d*\.\d[0-9]{1}$")))
                {
                    UnnecessaryСadUniqueIdModels(rawData);
                    return null;
                }

                regex = new Regex(@"\d*\.\d*\.\d{2}");
                matches = regex.Matches(rawData);
                if(matches.Count != 0)
                {
                    match = matches[0].Value;
                }
                else
                {
                    UnnecessaryСadUniqueIdModels(rawData);
                    return null;
                }
                                
            }
            catch (Exception ex)
            {
                WriteError("CADUniqueIdClass/GetCADUniq -> " + ex.Message);
            }
            

            string[] splitter = { match };
            splitter = rawData.Split(splitter, StringSplitOptions.RemoveEmptyEntries);           

            var dimensions = match.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                double rw_2000 = Convert.ToDouble(dimensions[0] + "0") / 2000;
                double rh_2000 = Convert.ToDouble(dimensions[1] + "0") / 2000;

                double rw_1690 = Convert.ToDouble(dimensions[0] + "0") / 1690;
                double rh_1690 = Convert.ToDouble(dimensions[1] + "0") / 1690;

                double rw_1400 = Convert.ToDouble(dimensions[0] + "0") / 1400;
                double rh_1400 = Convert.ToDouble(dimensions[1] + "0") / 1400;

                double rw_1000 = Convert.ToDouble(dimensions[0] + "0") / 1000;
                double rh_1000 = Convert.ToDouble(dimensions[1] + "0") / 1000;

                cadModel = new CADUniqueIdModel()
                {
                    Name = splitter[0],
                    Height = Convert.ToInt32(dimensions[1]),
                    Width = Convert.ToInt32(dimensions[0]),
                    Depth = dimensions.Count() > 2 ? Convert.ToInt32(dimensions[2]) : 0,
                    RatioWidth_2000 = rw_2000,
                    RatioHeight_2000 = rh_2000,
                    RatioWidth_1690 = rw_1690,
                    RatioHeight_1690 = rh_1690,
                    RatioWidth_1400 = rw_1400,
                    RatioHeight_1400 = rh_1400,
                    RatioWidth_1000 = rw_1000,
                    RatioHeight_1000 = rh_1000
                };
            }
            catch (Exception ex)
            {
                WriteError("GetCADUniq -> " + ex.Message);
                return null;
            }
            


            return cadModel;
        }

        private static int[] GetUniqueDepth(List<CADUniqueIdModel> cads)
        {
            int[] result = cads.Select(x => x.Depth).ToArray();
            result = result.Distinct().ToArray();

            Array.Sort(result);
            
            return result;
        }

        private static DataRow[] SqlQueryLeit(string query, string connectionStr)
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
        
        public static List<SumCADUnique> GetSumCADUniques()
        {
            return sumCADs;
        }

        public static DataTable ToDataTable<T>(IList<T> data)
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

        private static void UnnecessaryСadUniqueIdModels(string rawData)
        {            

            Regex regex = new Regex(@"\d*\.\d*");
            MatchCollection matches = regex.Matches(rawData);
            string match = matches[1].Value;
            string[] spliter = { match };
            spliter = rawData.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
            var dimension = match.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            unnecessaryСadUniqueIdModels.Add(new CADUniqueIdModel()
            {
                Name = spliter[0] + " рассчет не произведен",
                Height = Convert.ToInt32(dimension[1]),
                Width = Convert.ToInt32(dimension[0]),
                Depth = 0,
                RatioWidth_2000 = 0,
                RatioHeight_2000 = 0,
                RatioWidth_1690 = 0,
                RatioHeight_1690 = 0,
                RatioWidth_1400 = 0,
                RatioHeight_1400 = 0,
                RatioWidth_1000 = 0,
                RatioHeight_1000 = 0

            });
        }

        public static List<CADUniqueIdModel> GetCollectionCADUnique()
        {

            List<CADUniqueIdModel> cadUniqueIdModels = new List<CADUniqueIdModel>();
            try
            {
                cadUniques = SqlQueryLeit("SELECT DISTINCT  CADUniqueId FROM Leit_PU", connectionStr);
            }
            catch (Exception ex)
            {
                WriteError("CADUniqueIdClass/cadUniques = SqlQueryLeit(SELECT DISTINCT  CADUniqueId FROM Leit_PU, connectionStr) -> " + ex.Message);
                MessageBox.Show("Нет подключения к БД!");
            }

            string rawData = string.Empty;

            if (cadUniques.Any())
            {
                foreach (DataRow d in cadUniques)
                {
                    rawData = d.ItemArray.First().ToString();
                    if (rawData != string.Empty && rawData != "1ZZZZZZZZZZZZZZZZZZZ2ZZZZZZZZZZZZZZZZZZZ")
                    {
                        var item = GetCADUniq(rawData);
                        if (item != null)
                        {
                            cadUniqueIdModels.Add(item);
                        }
                    }

                }

                int[] depths = GetUniqueDepth(cadUniqueIdModels);

                for (int i = 0; i < depths.Length; i++)
                {
                    int sumWidth2000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_2000));
                    int sumWidth1690 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_1690));
                    int sumWidth1400 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_1400));
                    int sumWidth1000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_1000));
                    int sumHeight2000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_2000));
                    int sumHeight1690 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_1690));
                    int sumHeight1400 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_1400));
                    int sumHeight1000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_1000));
                    int countItems = cadUniqueIdModels.FindAll(x => x.Depth == depths[i]).Count;
                    sumCADs.Add(new SumCADUnique() { Name = "Глубина " + depths[i], CountItems = countItems, SumHeightBy1000 = sumHeight1000, SumHeightBy1400 = sumHeight1400, SumHeightBy1690 = sumHeight1690, SumHeightBy2000 = sumHeight2000, SumWidthBy1000 = sumWidth1000, SumWidthBy1400 = sumWidth1400, SumWidthBy1690 = sumWidth1690, SumWidthBy2000 = sumWidth2000 });
                }

                cadUniqueIdModels.AddRange(unnecessaryСadUniqueIdModels);

            }


            return cadUniqueIdModels;
        }

        public static List<CADUniqueIdModel> GetCollectionCADUnique(List<string> cadNames)
        {

            List<CADUniqueIdModel> cadUniqueIdModels = new List<CADUniqueIdModel>();

            if (cadNames.Any())
            {
                foreach (string s in cadNames)
                {
                 
                     var item = GetCADUniq(s);
                     if (item != null)
                     {
                         cadUniqueIdModels.Add(item);
                    }
                    else
                    {
                        return null;
                    }                  

                }

                int[] depths = GetUniqueDepth(cadUniqueIdModels);

                for (int i = 0; i < depths.Length; i++)
                {
                    int sumWidth2000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_2000));
                    int sumWidth1690 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_1690));
                    int sumWidth1400 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_1400));
                    int sumWidth1000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioWidth_1000));
                    int sumHeight2000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_2000));
                    int sumHeight1690 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_1690));
                    int sumHeight1400 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_1400));
                    int sumHeight1000 = cadUniqueIdModels.Where(x => x.Depth == depths[i]).Sum(x => (int)Math.Round(x.RatioHeight_1000));
                    int countItems = cadUniqueIdModels.FindAll(x => x.Depth == depths[i]).Count;
                    sumCADs.Add(new SumCADUnique() { Name = "Глубина " + depths[i], CountItems = countItems, SumHeightBy1000 = sumHeight1000, SumHeightBy1400 = sumHeight1400, SumHeightBy1690 = sumHeight1690, SumHeightBy2000 = sumHeight2000, SumWidthBy1000 = sumWidth1000, SumWidthBy1400 = sumWidth1400, SumWidthBy1690 = sumWidth1690, SumWidthBy2000 = sumWidth2000 });
                }

                cadUniqueIdModels.AddRange(unnecessaryСadUniqueIdModels);

            }


            return cadUniqueIdModels;
        }

        public static void WriteError(string message)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(fName, FileMode.Append, FileAccess.Write), Encoding.UTF8))
            {
                sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + " - ", message));
            }
        }
    }
}
