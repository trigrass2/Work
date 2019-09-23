using System;
using Android.App;
using Firebase.Iid;
using Android.Util;

namespace Vertical.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
#pragma warning disable CS0618 // Тип или член устарел
    public class MyFirebaseIIDService : FirebaseInstanceIdService
#pragma warning restore CS0618 // Тип или член устарел
    {
        const string TAG = "MyFirebaseIIDService";
#pragma warning disable CS0672 // Член переопределяет устаревший член
        public override void OnTokenRefresh()
#pragma warning restore CS0672 // Член переопределяет устаревший член
        {
#pragma warning disable CS0618 // Тип или член устарел
            var refreshedToken = FirebaseInstanceId.Instance.Token;
#pragma warning restore CS0618 // Тип или член устарел
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}