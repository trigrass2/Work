using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.Views;
using Com.OneSignal;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ServiceDesk
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzA1MzJAMzEzNjJlMzQyZTMwQ0h1aGdRZ0oybTJ2NDVmU1hvVDkxdkorLzJaZjdWbWlpbGx0M3RUN1dpVT0=");

            InitializeComponent();

            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1").EndInit();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            
        }
    }
}
