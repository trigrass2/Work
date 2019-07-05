using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Vertical.Droid
{
    [Activity(Label = "Вертикаль", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            Xamarin.Forms.Svg.Droid.SvgImage.Init(this);

            LoadApplication(new App());
        }
    }
}