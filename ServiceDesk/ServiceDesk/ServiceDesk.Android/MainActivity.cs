using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Com.OneSignal;

namespace ServiceDesk.Droid
{
    [Activity(Label = "ПИК-Заявка", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1").EndInit();

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent != null)
            {
                var args = intent.GetStringExtra("args");
            }
        }
    }
}