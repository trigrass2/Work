using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using ZXing.Mobile;
using ZXing;

namespace AddingProductUnit
{
	
	public partial class PopUpPage : ContentPage
	{
        public ZXingScannerView scanner;
        private StackLayout mainStack = new StackLayout();
        public PopUpPage ()
		{
			InitializeComponent ();            
            
            backButton.Clicked += OnClickScan;
            ScanAsync();
            mainStack.Children.Add(scanner);
            mainStack.Children.Add(backButton);
            Content = mainStack;
        }

        public void OnClickScan(object sender, EventArgs e)
        {
            Navigation.PopAsync();            
        }
        Button backButton = new Button()
        {
            Text = "Назад",
            
            TextColor = Color.White,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.Red                       
        };

        public void ScanAsync()
        {
            scanner = new ZXingScannerView();
            MobileBarcodeScanningOptions scanningOptions = new MobileBarcodeScanningOptions
            {                
                UseNativeScanning = true,
                UseFrontCameraIfAvailable = true,
                PossibleFormats = new List<BarcodeFormat>(),
                TryHarder = true,
                AutoRotate = false,
                
            };            
            scanner.Options = scanningOptions;
            scanner.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            scanner.Options.PossibleFormats.Add(BarcodeFormat.DATA_MATRIX);
            scanner.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);
            
            scanner.OnScanResult += (result) => {
                
                scanner.IsScanning = false;
                if (scanner.IsScanning)
                {
                    scanner.AutoFocus();
                }                              

                Device.BeginInvokeOnMainThread(async () => {
                    if (result != null)
                    {
                       bool scanOK = await DisplayAlert("Полученное изделие", result.Text, "OK", "Cancel");
                        if (scanOK)
                        {
                            await Navigation.PopAsync();
                        }
                        else {
                            scanner.Result = null;
                            await Navigation.PopAsync();
                            }; 
                    }
                });                
            };           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            scanner.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            scanner.IsScanning = false;

            base.OnDisappearing();
        }
    }
}

