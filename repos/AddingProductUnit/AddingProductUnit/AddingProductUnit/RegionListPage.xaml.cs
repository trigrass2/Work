using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace AddingProductUnit
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegionListPage : ContentPage
	{
        private PopUpPage scanPage;
        private SfListView listView;
        private List<ProductRegion> Regions { get; set; }
        private ZXingScannerView scanner = new ZXingScannerView();
        private StackLayout baseLayout;

        public RegionListPage (List<ProductRegion> regions)
		{
			InitializeComponent ();
            
            Regions = new List<ProductRegion>(regions);
            BackgroundColor = Color.FromRgb(70, 68, 81);

            listView = new SfListView()
            {
                ItemsSource = Regions,
                ItemSpacing = new Thickness(5, 5, 5, 5),                
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Button
                    {
                        BackgroundColor = Color.FromHex("#5990c6"),
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                        BorderWidth = 1,
                        FontFamily = "Arial",
                        CornerRadius = 8,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };
                    label.SetBinding(Button.TextProperty, "Name");
                    return label;
                })

            };
            Button scanBtn = new Button()
            {
                BackgroundColor = Color.White,
                Text = "Сканировать новое изделие",
                FontSize = 19,
                CornerRadius = 8,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                Margin = new Thickness(5, 10, 5, 10)                
            };
            scanBtn.Clicked += OnClickStart;
            listView.SelectionChanging += ListView_SelectionChanging;

            baseLayout = new StackLayout();
            baseLayout.Children.Add(listView);
            baseLayout.Children.Add(scanBtn);
            
            Content = baseLayout;
        }

        private async void ListView_SelectionChanging(object sender, ItemSelectionChangingEventArgs e)
        {
            if( scanPage != null && scanPage.scanner != null && scanPage.scanner.Result != null)
            {
                ProductRegion needItem = (ProductRegion)e.AddedItems[0];

                if (e.AddedItems.Count > 0 && e.AddedItems[0] == Regions[0])
                    e.Cancel = true;

                bool result = await DisplayAlert("Подтвердить действие", "Добавить изделие?", "Да", "Нет");

                if (result)
                {
                    string statusRequest = RestApi.NewProductEvent(scanPage.scanner.Result.Text, needItem.Region_id);
                    Message(statusRequest);
                }                
            }            
        }

        private void Message(string status)
        {
            DisplayAlert("Статус отправки", status, "OK");
        }

        public async void OnClickStart(object sender, EventArgs e)
        {
            scanPage = new PopUpPage();
            await Navigation.PushAsync(scanPage);

        }        

    }
}