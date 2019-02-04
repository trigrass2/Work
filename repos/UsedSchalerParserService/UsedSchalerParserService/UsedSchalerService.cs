using System.ServiceProcess;
using System.Threading;
using UsedSchalerParserLogic;

namespace UsedSchalerParserService
{
    public partial class UsedSchalerService : ServiceBase
    {
        public LogParser logParser;

        public UsedSchalerService()
        {
            InitializeComponent();
            logParser = new LogParser();
            ServiceName = "UsedSchalerService";
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            Thread loggerThread = new Thread(new ThreadStart(logParser.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logParser.Stop();
            Thread.Sleep(1000);
        }
    }
}
