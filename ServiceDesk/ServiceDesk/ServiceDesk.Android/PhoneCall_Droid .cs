using System;

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
        public void MakeQuickCall(string PhoneNumber)
        {
            try
            {
                var uri = Android.Net.Uri.Parse(string.Format("tel:{0}", PhoneNumber));
                var intent = new Intent(Intent.ActionCall, uri);
                Forms.Context.StartActivity(intent);
            }
            catch (Exception ex)
            {
                new AlertDialog.Builder(Android.App.Application.Context).SetPositiveButton("OK", (sender, args) =>
                {
                    //User pressed OK
                })
                .SetMessage(ex.ToString())
                .SetTitle("Android Exception")
                .Show();
            }
        }

    }
}