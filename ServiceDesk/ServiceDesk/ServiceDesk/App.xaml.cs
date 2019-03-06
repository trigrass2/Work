using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ServiceDesk
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzA1MzJAMzEzNjJlMzQyZTMwQ0h1aGdRZ0oybTJ2NDVmU1hvVDkxdkorLzJaZjdWbWlpbGx0M3RUN1dpVT0=");

            InitializeComponent();

            MainPage = new NavigationPage(new MenuPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
