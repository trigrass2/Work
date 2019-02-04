using PdfSeeker;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace SeekerPdfService
{
    public partial class Service1 : ServiceBase
    {
        private static readonly string Directory = ConfigurationManager.ConnectionStrings["Directory"].ConnectionString;
        private static readonly string Printer = ConfigurationManager.ConnectionStrings["Printer"].ConnectionString;
        private static readonly string DeleteOrNotDelete = ConfigurationManager.ConnectionStrings["DeleteOrNotDelete"].ConnectionString;
        Seeker seeker = new Seeker(Directory, Printer, DeleteOrNotDelete);

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;

        }

        protected override void OnStart(string[] args)
        {
            Thread serviceThread = new Thread(new ThreadStart(seeker.Start));
            serviceThread.Start();
        }

        protected override void OnStop()
        {
            seeker.Stop();
            Thread.Sleep(1000);
        }
    }
}
