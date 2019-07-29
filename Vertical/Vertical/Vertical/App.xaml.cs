using Vertical.Views;
using Xamarin.Forms;
using Xamarin.Forms.Svg;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Vertical
{
    public partial class App : Application
    {
        public App()
        {
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTIxODUxQDMxMzcyZTMyMmUzMGF5QTk0L0pZM2RxL0NKWDhtT3JySjZMU2QxY2lrcFNpTVEyL1E0OG9mUnc9");
            InitializeComponent();
            
            MainPage = new NavigationPage(new AutorizationsPage());
        }

        protected override void OnStart()
        {
            SvgImageSource.RegisterAssembly();
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
