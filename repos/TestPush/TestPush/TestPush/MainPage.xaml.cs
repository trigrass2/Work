using Com.OneSignal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestPush
{
    
    /// <summary>
    /// Основная страница
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private bool fromparent;
        private StackLayout SubList;

        public MainPage()
        {
            InitializeComponent();
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
                        HorizontalOptions = LayoutOptions.Center
                    };
                    switcher.Toggled += delegate (object sender, ToggledEventArgs e)
                    {
                        if (e.Value)
                            OneSignal.Current.SendTag(sb.Id.ToString(), "testTag");
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
                    new SubButton(1,"tegOne"),
                    new SubButton(2,"tegTwo"),
                    new SubButton(3,"tegTree")
                };
            });
            
        }
    }
}
