using System;
using System.Collections.Generic;
using Vertical.Droid;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomTransitionsRenderer))]
namespace Vertical.Droid
{
#pragma warning disable CS0618 // Тип или член устарел
    public class CustomTransitionsRenderer : Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer
    {
        //SwitchContentAsync
        protected override void SetupPageTransition(Android.Support.V4.App.FragmentTransaction transaction, bool isPush)
        {
            if (isPush)
                transaction.SetCustomAnimations(Resource.Animator.move_in_left, 0);
            else //prevView enter:
                transaction.SetCustomAnimations(Resource.Animator.slide_in_right, 0);
        }
    }
#pragma warning restore CS0618 // Тип или член устарел
}