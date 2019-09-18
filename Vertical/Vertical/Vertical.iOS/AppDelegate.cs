using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;

namespace Vertical.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Xamarin.Forms.Svg.iOS.SvgImage.Init();
            Distribute.DontCheckForUpdatesInDebug();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
