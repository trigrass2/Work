using System.ServiceProcess;

namespace UsedSchalerParserService
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new UsedSchalerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
