using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.AppCenter.Distribute;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Vertical.Services;

namespace Vertical.Droid
{
    [Activity(Label = "ПИК-Система", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);
           
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            IsPlayServicesAvailable();

            CreateNotificationChannel();

            Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            UserDialogs.Init(this);
            Xamarin.Forms.Svg.Droid.SvgImage.Init(this);

            Distribute.SetEnabledForDebuggableBuild(false);

            LoadApplication(new App());
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                Loger.WriteMessageAsync(LogPriority.Error, "Сервисы гугл не доступны");
                return false;
            }
            else
            {                
                return true;
            }
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {                
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}