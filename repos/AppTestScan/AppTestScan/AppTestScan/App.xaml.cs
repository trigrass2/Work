using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppTestScan
{
    public static class AppGlobals
    {
        public static object refCurrentPageContext;
        public static bool refMicrophoneFeature;
        public static MainPage refMainPageContext;
        public static Page_GoodsList refPage_GoodList;
    }

    public partial class App : Application
    {
        public App(ref object CurrentPageContext, ref bool locMicrophoneFeature)
        {
            InitializeComponent();

            AppGlobals.refCurrentPageContext = CurrentPageContext;

            AppGlobals.refCurrentPageContext = new MainPage();
            MainPage = new NavigationPage((MainPage)AppGlobals.refCurrentPageContext);

            AppGlobals.refMicrophoneFeature = locMicrophoneFeature;
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
