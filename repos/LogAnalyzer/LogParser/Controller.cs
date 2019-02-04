using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogParser
{
    public static class Controller
    {
        public static List<LogFileModel> GetTable(string fileName)
        {            
            List<LogFileModel> result = new List<LogFileModel>();
            try
            {
                using (StreamReader fs = new StreamReader(fileName))
                {
                    while (true)
                    {
                        string temp = fs.ReadLine();

                        if (temp == null) break;

                        string[] t = temp.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
                        result.Add(
                            new LogFileModel
                            {
                                IpUser = t[0],
                                UserName = t[1],
                                Date = t[2],
                                Time = t[3],
                                ServiceAndSample = t[4],
                                NamePC = t[5],
                                DestinationAddress = t[6],
                                LeadTime = Convert.ToInt32(t[7]),
                                ReceivedByte = Convert.ToInt32(t[8]),
                                SentBytes = Convert.ToInt32(t[9]),
                                CodeServiceState = Convert.ToInt32(t[10]),
                                Windows2000StatusCode = Convert.ToInt32(t[11]),
                                RequestType = t[12],
                                OperationObject = t[13],
                                ParamQuery = t[14]
                                
                            }
                           );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " LogParser/GetTable<T>(string fileName)");
            }
            

            return result;
        }
    }
}
