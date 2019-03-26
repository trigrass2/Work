using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.Views;
using Com.OneSignal;
using System;
using ServiceDesk.PikApi;
using ServiceDesk.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ServiceDesk
{
    public partial class App : Application
    {
        public static string isNotified;
        public static string isNotifiedId;
        public bool navigating;
        public bool IsNotified;

        public App(bool shallNavigate)
        {
            navigating = shallNavigate;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzA1MzJAMzEzNjJlMzQyZTMwQ0h1aGdRZ0oybTJ2NDVmU1hvVDkxdkorLzJaZjdWbWlpbGx0M3RUN1dpVT0=");

            InitializeComponent();
            
            //MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1")
                             .EndInit();
            OnAppStart();
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {

        }

        private async void OnAppStart()
        {
            #region PushNotication
            try
            {
                if (navigating == false)
                {
                    if (ServiceDeskApi.AccessToken != null)
                    {
                        if (IsNotified == true)
                        {
                            MainPage = new NavigationPage(new MenuPage());
                        }
                        else
                        {
                            if (ServiceDeskApi.AccessToken != null)
                            {
                                MainPage = new NavigationPage(new MenuPage());//new SelectedTaskPage(new ViewModels.TaskViewModel(new ServiceDesk_TaskListView()));
                                IsNotified = true;
                            }
                            else
                            {
                                MainPage = new NavigationPage(new StartPage());
                            }
                        }
                    }
                    else
                    {
                        MainPage = new NavigationPage(new StartPage());
                    }
                }
                else
                {
                    if (ServiceDeskApi.AccessToken != null)
                    {
                        MainPage = new NavigationPage(new MenuPage());//new SelectedTaskPage(new ViewModels.TaskViewModel(new ServiceDesk_TaskListView()));
                    }
                    else
                    {
                        MainPage = new NavigationPage(new StartPage());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            #endregion
        }
    }
}
