using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;

namespace ServiceDesk.Droid
{
    [Activity(Label = "ПИК-Заявка", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //}

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
    }
}