using Android.App;
using Android.Support.V7.App;

namespace ServiceDesk.Droid
{
    [Activity(Label = "ПИК-Индустрия СД", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}