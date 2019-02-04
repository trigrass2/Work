using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser
{
    public class LogFileModel
    {
        public string IpUser { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string OperationObject { get; set; }
        public string ParamQuery { get; set; }
        public string RequestType { get; set; }
        public int CodeServiceState { get; set; }
        public string ServiceAndSample { get; set; }
        public string NamePC { get; set; }
        public string DestinationAddress { get; set; }
        public int LeadTime { get; set; }
        public int ReceivedByte { get; set; }
        public int SentBytes { get; set; }        
        public int Windows2000StatusCode { get; set; }        
        
    }
}
