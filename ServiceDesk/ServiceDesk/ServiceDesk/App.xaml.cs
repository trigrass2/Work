using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.Views;
using Com.OneSignal;
using System;
using ServiceDesk.PikApi;
using ServiceDesk.Models;
using Com.OneSignal.Abstractions;
using System.Collections.Generic;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ServiceDesk
{
    public partial class App : Application
    {
        public static string isNotified;
        public static string isNotifiedId;
        public static bool navigating;
        public static bool IsNotified;        

        private static ServiceDesk_TaskListView serviceDesk_TaskListView;

        public App(bool shallNavigate)
        {
            navigating = shallNavigate;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzA1MzJAMzEzNjJlMzQyZTMwQ0h1aGdRZ0oybTJ2NDVmU1hvVDkxdkorLzJaZjdWbWlpbGx0M3RUN1dpVT0=");

            InitializeComponent();

            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1")
                             .HandleNotificationOpened(HandleNotificationOpened)
                             .EndInit();
            try
            {
                if (serviceDesk_TaskListView != null)
                {
                    MainPage = new NavigationPage(new MenuPage());
                    App.Current.MainPage.Navigation.PushAsync(new SelectedTaskPage(new ViewModels.TaskViewModel(serviceDesk_TaskListView)));
                    serviceDesk_TaskListView = null;
                }
                else if (ServiceDeskApi.AccessToken != null)
                {
                    MainPage = new NavigationPage(new MenuPage());
                }
                else MainPage = new NavigationPage(new StartPage());
            }
            catch (Exception)
            {
                MainPage = new NavigationPage(new StartPage());
            }
            
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {            
            if (serviceDesk_TaskListView != null)
            {
                MainPage = new NavigationPage(new MenuPage());
                App.Current.MainPage.Navigation.PushAsync(new SelectedTaskPage(new ViewModels.TaskViewModel(serviceDesk_TaskListView)));
                serviceDesk_TaskListView = null;
            }else if(ServiceDeskApi.AccessToken != null)
            {
                MainPage = new NavigationPage(new MenuPage());
            }else MainPage = new NavigationPage(new StartPage());
        }

        //private async void OnAppStart()
        //{
        //    #region PushNotication
        //    try
        //    {
        //        if (navigating == false)
        //        {
        //            if (ServiceDeskApi.AccessToken != null)
        //            {
        //                if (IsNotified == true)
        //                {
        //                    MainPage = new NavigationPage(new MenuPage());
        //                }
        //                else
        //                {
        //                    if (ServiceDeskApi.AccessToken != null)
        //                    {
        //                        MainPage = new NavigationPage(new MenuPage());
        //                        await App.Current.MainPage.Navigation.PushAsync(new SelectedTaskPage(new ViewModels.TaskViewModel(serviceDesk_TaskListView)));
        //                        IsNotified = true;
        //                        serviceDesk_TaskListView = null;
        //                    }
        //                    else
        //                    {
        //                        MainPage = new NavigationPage(new StartPage());
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MainPage = new NavigationPage(new StartPage());
        //            }
        //        }
        //        else
        //        {
        //            if (ServiceDeskApi.AccessToken != null)
        //            {
        //                MainPage = new NavigationPage(new MenuPage());
        //                await App.Current.MainPage.Navigation.PushAsync(new SelectedTaskPage(new ViewModels.TaskViewModel(serviceDesk_TaskListView)));
        //                serviceDesk_TaskListView = null;
        //            }
        //            else
        //            {
        //                MainPage = new NavigationPage(new StartPage());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    #endregion
        //}

        private static void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            OSNotificationPayload payload = result.notification.payload;
            Dictionary<string, object> additionalData = payload.additionalData;
            string taskId = string.Empty;

            if(additionalData != null && additionalData.Count != 0)
            {
                foreach (var a in additionalData)
                {
                    taskId = a.Value.ToString();
                }

                var d = ServiceDeskApi.GetData<ServiceDesk_TaskListView>(ServiceDeskApi.ApiEnum.GetTasks);
                serviceDesk_TaskListView = d.Where(x => x.Task_id == int.Parse(taskId)).FirstOrDefault();
            }
                       
        }
    }
}
