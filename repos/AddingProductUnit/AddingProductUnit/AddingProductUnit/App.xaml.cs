using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AddingProductUnit
{

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //var startPage = new PopUpPage();
            //MainPage = new NavigationPage(startPage);
            MainPage = new NavigationPage(new MainPage());
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
