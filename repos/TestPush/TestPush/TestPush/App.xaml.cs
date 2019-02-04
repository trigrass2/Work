using Com.OneSignal;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TestPush
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new AuthPage());
            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1").EndInit();
        }

        protected override void OnStart()
        {
            OneSignal.Current.RegisterForPushNotifications();
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
