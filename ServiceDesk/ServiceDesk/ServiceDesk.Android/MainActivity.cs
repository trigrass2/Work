using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Com.OneSignal;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;

namespace ServiceDesk.Droid
{
    [Activity(Label = "ПИК-Индустрия СД", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1").EndInit();
           
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 0);
            }

            string JobId = Intent.GetStringExtra("JobID");
            string NotificationId = Intent.GetStringExtra("NotificationId");

            if (JobId == null || NotificationId == null)
            {
                LoadApplication(new App(false));
            }
            else
            {
                App.isNotified = JobId;
                App.isNotifiedId = NotificationId;

                LoadApplication(new App(true));
            }
            //LoadApplication(new App());
        }       
    }
}