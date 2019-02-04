using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataCollector
{
    public class MasterOfData
    {

        private AwmRepDurToSql durToSql;
        private AwmRepToSql repToSql;
        private AwmHmimeshToSql awmHmimesh;
        private BackgroundWorker bw_rep;
        private BackgroundWorker bw_log;
        private BackgroundWorker bw_dur;
        

        public MasterOfData()
        {
            
            durToSql = new AwmRepDurToSql();
            repToSql = new AwmRepToSql();
            awmHmimesh = new AwmHmimeshToSql();

            bw_rep = new BackgroundWorker();
            bw_rep.DoWork += repToSql.StartModule;

            bw_log = new BackgroundWorker();
            bw_log.DoWork += awmHmimesh.StartModule;

            bw_dur = new BackgroundWorker();
            bw_dur.DoWork += durToSql.StartModule;
        }

        public void StartModule()
        {
            bw_rep.RunWorkerAsync();
            bw_log.RunWorkerAsync();
            bw_dur.RunWorkerAsync();
        }

        public void StopModule()
        {
            awmHmimesh.StopModule();
            durToSql.StopModule();
            repToSql.StopModule();
        }
    }
}
