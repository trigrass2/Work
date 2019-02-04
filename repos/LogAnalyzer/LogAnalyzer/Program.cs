using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogAnalyzer
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDczMjhAMzEzNjJlMzMyZTMwUDdkL2FpeVcxdFJ1VW01NHBtYlJ5cktmNHNTMTh2a1REMlh1NlpTRVNtdz0=;NDczMjlAMzEzNjJlMzMyZTMwaGVDTXRQbGJxb3RyZjMyRXc0L2F1akhCdlQ3OGNkUlNNMElqNWQ0YW9qND0==");
            Application.Run(new Form1());
        }
    }
}
