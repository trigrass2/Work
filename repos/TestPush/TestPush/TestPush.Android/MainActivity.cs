using Android.App;
using Android.Content.PM;
using Android.OS;
using Com.OneSignal;

namespace TestPush.Droid
{
    [Activity(Label = "TestPush", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            OneSignal.Current.StartInit("8cacfbb9-a453-41c4-a4a0-c98dce5721a1").EndInit();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}