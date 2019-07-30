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
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTI2NjA3QDMxMzcyZTMyMmUzMEhDTERkazVSSTB1VW8vVFpheGF0VXNYMS93by9JM2E5TVVrQStRc21mR1k9");
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
