using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;

namespace UsedSchalerParserService
{
    [RunInstaller(true)]
    public partial class InstallerUsedSchalerParserService : Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public InstallerUsedSchalerParserService()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Username = "Ilgar Mamishov (MamishovIN@gmail.com)",
                Password = "ac2208646"
            };
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.Description = "Копирует данные из файла UsedSchaler.old в БД";
            serviceInstaller.ServiceName = "UsedSchalerService";
            
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
