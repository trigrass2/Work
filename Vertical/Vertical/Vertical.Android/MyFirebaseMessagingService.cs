using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;

namespace Vertical.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            var notify = remoteMessage.GetNotification();
            string title = notify.Title;
            string text = notify.Body;
            string color = notify.Color;

            ShowNotification(title, text, color);
        }

        void ShowNotification(string title, string text, string color)
        {
            NotificationCompat.Builder mNotify = new NotificationCompat.Builder(this, "");
            mNotify.SetLights(Notification.ColorDefault, 100, 200);
            //mNotify.SetSmallIcon(Notifica);
            mNotify.SetContentTitle(title);
            mNotify.SetContentText(text);
            //mNotify.SetDefaults(Notification.AudioAttributesDefault);
            System.Diagnostics.Debug.WriteLine(text);
            NotificationManager mNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

            //int mId = 1001;
            //try { mNotificationManager.notify(mId, mNotify.build()); }
            //catch (Exception e) { e.printStackTrace(); }
        }
    }
}