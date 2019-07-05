using Android.App;
using Android.Views;
using Vertical.CustomViews;
using Vertical.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarImplementation))]
namespace Vertical.Droid
{
    public class StatusBarImplementation : IStatusBar
    {
        public StatusBarImplementation()
        {
        }

        WindowManagerFlags _originalFlags;        

        public void HideStatusBar()
        {
            #pragma warning disable CS0618 // Тип или член устарел
            var activity = (Activity)Forms.Context;
            #pragma warning restore CS0618 // Тип или член устарел

            var attrs = activity.Window.Attributes;
            _originalFlags = attrs.Flags;
            attrs.Flags |= WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attrs;
        }

        public void ShowStatusBar()
        {
            #pragma warning disable CS0618 // Тип или член устарел
            var activity = (Activity)Forms.Context;
            #pragma warning restore CS0618 // Тип или член устарел

            var attrs = activity.Window.Attributes;
            attrs.Flags = _originalFlags;
            activity.Window.Attributes = attrs;
        }
    }
}