using System;
using System.Windows;
using System.Windows.Threading;

namespace Stations
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger logger = new Logger();
            logger.Write($"{DateTime.Now} {e.Exception.Message}");
        }
    }
}
