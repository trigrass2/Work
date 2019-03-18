using Com.OneSignal;
using ServiceDesk.Models;
using ServiceDesk.Models.Push;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using ServiceDesk.PikApi;
using System.Linq;

namespace ServiceDesk.Views
{
    public partial class SettingsPage : ContentPage
    {
        private bool fromparent;
        private StackLayout SubList;
        private ApplicationUser _user;
       
        public SettingsPage ()
		{
			InitializeComponent ();
            
            _user = ServiceDeskApi.GetUser<ApplicationUser>(ServiceDeskApi.ApiEnum.GetUserInfo);
            SubList = new StackLayout();
            Content = SubList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            fromparent = true;
            if (fromparent)
            {
                OneSignal.Current.GetTags(TagsReceived);
            }
        }

        private void TagsReceived(Dictionary<string, object> tags)
        {
            if (tags == null)
            {
                tags = new Dictionary<string, object>();
            }
            MakeSublistAsync(tags);
        }

        private async void MakeSublistAsync(Dictionary<string, object> tags)
        {
            try
            {                
                List<SubButton> subscriptions = await GetSubsAsync();
                if (subscriptions == null)
                    return;
                foreach (SubButton sb in subscriptions)
                {
                    StackLayout stack = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 0,
                        Padding = 10
                    };
                    Label label = new Label
                    {
                        Text = sb.Label,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    Switch switcher = new Switch
                    {
                        IsToggled = tags.ContainsKey(sb.Id.ToString()),
                        OnColor = Color.Green,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    switcher.Toggled += delegate (object sender, ToggledEventArgs e)
                    {
                        if (e.Value)
                            OneSignal.Current.SendTag(sb.Id.ToString(), sb.Label);
                        else
                            OneSignal.Current.DeleteTag(sb.Id.ToString());
                    };
                    stack.Children.Add(label);
                    stack.Children.Add(switcher);
                    await Task.Run(() =>
                    {
                        var tcs = new TaskCompletionSource<bool>();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            SubList.Children.Add(stack);
                            tcs.SetResult(false);
                        });
                        return tcs.Task;
                    });
                }

                return;
            }
            catch (Exception e)
            {
                await DisplayAlert("ERROR", e.ToString(), "OK");
                return;
            }
        }

        private async Task<List<SubButton>> GetSubsAsync()
        {
            return await Task.Run(() => {
                return new List<SubButton>
                {
                    new SubButton(_user.Id,"User_id")
                };
            });

        }
    }
}