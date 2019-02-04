using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SeekerPdfService
{
    [RunInstaller(true)]
    public partial class InstallerSeeker : Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public InstallerSeeker()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "SeekerPdfService";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
