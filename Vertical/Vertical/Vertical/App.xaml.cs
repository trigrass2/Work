using Vertical.Views;
using Xamarin.Forms;
using Xamarin.Forms.Svg;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Vertical
{
    public partial class App : Application
    {
        public App()
        {            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM0NzM0QDMxMzcyZTMyMmUzMEdvWk1ZSGp2R2hiY2NSWnR1emlrWVNmTXRKbVNVWDgvLzBYMGNHS2Q0NjA9");
            InitializeComponent();
            
            MainPage = new NavigationPage(new AutorizationsPage());
        }

        protected override void OnStart()
        {
            SvgImageSource.RegisterAssembly();
            AppCenter.Start($"{Constants.AndroidSecret}",
                  typeof(Analytics), typeof(Crashes));

            AppCenter.Start($"{Constants.AndroidSecret}", typeof(Distribute));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
