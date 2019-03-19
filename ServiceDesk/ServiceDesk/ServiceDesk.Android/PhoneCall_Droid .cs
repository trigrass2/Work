using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using ServiceDesk.Droid;
using ServiceDesk.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneCall_Droid))]
namespace ServiceDesk.Droid
{    
    public class PhoneCall_Droid : IPhoneCall
    {
        public Task MakeQuickCall(string PhoneNumber)
        {            
            var packageManager = Android.App.Application.Context.PackageManager;
            Android.Net.Uri telUri = Android.Net.Uri.Parse($"tel:{PhoneNumber}");
            var callIntent = new Intent(Intent.ActionCall, telUri);

            callIntent.AddFlags(ActivityFlags.NewTask);
            // проверяем доступность
            var result = null != callIntent.ResolveActivity(packageManager);

            if (!string.IsNullOrWhiteSpace(PhoneNumber) && result == true)
            {
                Android.App.Application.Context.StartActivity(callIntent);
                
            }

            return Task.FromResult(true);
        }

    }
}