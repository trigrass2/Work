using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using ZXing.Mobile;
using ZXing;
using System.Collections.ObjectModel;
using ToolsScanner.Model;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;

namespace ToolsScanner
{
    public partial class ScanPage : ContentPage
    {        
        public Grid mainGrid;
        public ZXingScannerView scanner = new ZXingScannerView();
        
        public ObservableCollection<Tool> ScannerTools { get; set; }
        public ObservableCollection<Tool> AllTools { get; set; }
        public ObservableCollection<Person> AllPersons { get; set; }
        public ObservableCollection<Person> BindingPersons { get; set; }
        public ObservableCollection<Person> ScannerPersons { get; set; }
        public string entryBarcode = string.Empty;

        private ListView listView;
        private StackLayout stackLayoutForScanButtons = new StackLayout() {Padding = new Thickness(5, 0, 5, 5), Margin = new Thickness(5, 5, 5, 5), Orientation = StackOrientation.Vertical };        

        Button backButton = new Button()
        {
            Text = "Назад",
            FontFamily = "Verdana",
            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
            TextColor = Color.White,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.Red,
            CornerRadius = 8            
        };

        Button completeButton = new Button()
        {            
            Text = "Готово",
            FontFamily = "Verdana",
            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
            TextColor = Color.FromHex("#464451"),
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromHex("#ffeeed"),
            CornerRadius = 8
        };

        Button getEntryButton = new Button()
        {            
            Text = "Ручной ввод",
            TextColor = Color.FromHex("#464451"),
            FontFamily = "Verdana",
            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromHex("#ffeeed"),
            CornerRadius = 8
        };

        Label titleLabel = new Label
        {
            TextColor = Color.White,
            FontSize = 15,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        Grid horizontalStacklayoutButtons = new Grid
        {
            Padding = new Thickness(0, 0, 0, 0),
           
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition{Width = new GridLength(1,GridUnitType.Star) },
                new ColumnDefinition{Width = new GridLength(1,GridUnitType.Star) },
                new ColumnDefinition{Width = new GridLength(1,GridUnitType.Star) }
            },
            
        };

        public ScanPage(ObservableCollection<Person> people)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            ScannerPersons = new ObservableCollection<Person>();
            BindingPersons = new ObservableCollection<Person>();
            AllPersons = people;

            mainGrid = new Grid();


            listView = new ListView()
            {
                SeparatorColor = Color.Gray,
                HasUnevenRows = true,
                ItemsSource = ScannerPersons,

                ItemTemplate = new DataTemplate(() =>
                {
                    Label idLabel = new Label { FontSize = 18, TextColor = Color.White };
                    idLabel.SetBinding(Label.TextProperty, "Person_id");

                    Label namePersonLabel = new Label { FontSize = 15, TextColor = Color.White };
                    namePersonLabel.SetBinding(Label.TextProperty, "Person_name");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { idLabel, namePersonLabel }
                        }
                    };

                })
            };

            completeButton.Clicked += CompleteClick;
            backButton.Clicked += BackClick;
            getEntryButton.Clicked += ToManualInput;
            Scan();
            Content = GetContent();
        }

        public ScanPage(ObservableCollection<Tool> allTools)
        {            
            NavigationPage.SetHasNavigationBar(this, false); 
            AllTools = allTools;
            
            ScannerTools = new ObservableCollection<Tool>();           

            mainGrid = new Grid()
            {
                BackgroundColor = Color.FromRgb(70, 68, 81),
                Padding = new Thickness(5, 5, 5, 5),
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star)},

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };

            
            listView = new ListView()
            {                 
                
                HasUnevenRows = true,
                ItemsSource = ScannerTools,
                ItemTemplate = new DataTemplate(()=> {
                    Label idLabel = new Label { FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.FromHex("#464451") };
                    idLabel.SetBinding(Label.TextProperty, "Tool_id");

                    Label nameToolLabel = new Label { FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), TextColor = Color.FromHex("#464451") };
                    nameToolLabel.SetBinding(Label.TextProperty, "Tool_name");

                    Label namePersonToolLabel = new Label { FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.FromHex("#464451") };
                    namePersonToolLabel.SetBinding(Label.TextProperty, "Person_name");
                    Frame frame = new Frame()
                    {
                        Content = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            Children = { nameToolLabel, idLabel, namePersonToolLabel }
                        },
                        BackgroundColor = Color.FromHex("#ffeeed"),
                        CornerRadius = 6,
                        Padding = new Thickness(5),
                        Margin = new Thickness(0, 0, 0, 5)
                    };
                    ViewCell viewCell = viewCell = new ViewCell
                    {
                        View = frame                        
                    };
                    
                    return viewCell;
                })
            };
            
            listView.ItemSelected += listView_ItemSelected;
            completeButton.Clicked += CompleteClick;
            getEntryButton.Clicked += ToManualInput;
            backButton.Clicked += BackClick;
            Scan();
            
            Content = GetContent();
        }
        
        public async void ToManualInput(object sender, EventArgs e)
        {
           
            entryBarcode = await InputBox(this.Navigation);
            if(entryBarcode != null)
            {
                if (AllTools != null && AllTools.Count != 0)
                {
                    if (AllTools.Any(x => x.Tool_id == entryBarcode))
                    {
                        ScannerTools.Add(AllTools.Where(x => x.Tool_id == entryBarcode).FirstOrDefault());
                    }
                    else ScannerTools.Add(new Tool { Person_name = "Нет ответственного", Tool_id = entryBarcode, Tool_name = "-" });
                }
                else
                {
                    if (AllPersons != null && AllPersons.Count != 0)
                    {
                        if (AllPersons.Any(x => x.Person_id == entryBarcode))
                        {
                            ScannerPersons.Clear();
                            ScannerPersons.Add(AllPersons.Where(x => x.Person_id == entryBarcode).FirstOrDefault());
                        }
                        else await DisplayAlert("Нет такого!", "Попробуйте еще раз", "Cancel");
                    }
                }
            }
            Content = GetContent();

        }

        public async void BackClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async void CompleteClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text.Equals("Готово"))
            {                
                scanner.IsScanning = false;
                button.BackgroundColor = Color.FromHex("#32CD32");
                button.Text = "Отправить";
                button.TextColor = Color.White;
            }
            else
            {
                if (button.Text.Equals("Отправить"))
                {
                    await Navigation.PopAsync();
                    if (ScannerTools != null && ScannerTools.Count != 0)
                    {
                        NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                        IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                        if (navStack[navPage.Navigation.NavigationStack.Count - 1] is MainPage homePage)
                        {
                            homePage.AddTools(ScannerTools);
                        }
                    }
                    if(ScannerPersons != null /*&& ScannerPersons.Count != 0*/)
                    {
                        NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                        IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                        if (navStack[navPage.Navigation.NavigationStack.Count - 1] is MainPage homePage)
                        {
                            homePage.AddPerson(ScannerPersons[0]);
                        }
                    }
                }
            }            
        }

        public void Scan()
        {
            try
            {
                scanner.Options = new MobileBarcodeScanningOptions()
                {
                    UseNativeScanning = true,
                    UseFrontCameraIfAvailable = true,
                    PossibleFormats = new List<BarcodeFormat>(),
                    TryHarder = true,
                    AutoRotate = false,
                    TryInverted = true,
                    UseCode39ExtendedMode = true,                    
                    DelayBetweenContinuousScans = 5000
                };

                scanner.Options.PossibleFormats.Add(BarcodeFormat.EAN_8);
                scanner.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);
                scanner.Options.PossibleFormats.Add(BarcodeFormat.CODE_128);

                scanner.OnScanResult += (result) => {


                    scanner.IsAnalyzing = false;
                    if (scanner.IsScanning)
                    {
                        scanner.AutoFocus();
                    }

                    Device.BeginInvokeOnMainThread(async () => {
                        if (result != null)
                        {                            
                            if(AllTools != null && AllTools.Count != 0)
                            {
                                if (AllTools.Any(x => x.Tool_id == result.Text))
                                {
                                    ScannerTools.Add(AllTools.Where(x => x.Tool_id == result.Text).FirstOrDefault());
                                }
                                else ScannerTools.Add(new Tool { Person_name = "Нет ответственного", Tool_id = result.Text, Tool_name = "-" });
                            }
                            else
                            {
                                if (AllPersons != null && AllPersons.Count != 0)
                                {
                                    if (AllPersons.Any(x => x.Person_id == result.Text))
                                    {
                                        ScannerPersons.Clear();
                                        ScannerPersons.Add(AllPersons.Where(x => x.Person_id == result.Text).FirstOrDefault());
                                    }
                                    else await DisplayAlert("Нет такого!", "Попробуйте еще раз", "Cancel");
                                }
                            }
                                                                                                    
                            Content = GetContent();
                        }
                    });
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        private Grid GetContent()
        {
            scanner.IsScanning = true;

            Grid grid = new Grid()
            {
                BackgroundColor = Color.FromRgb(70, 68, 81),
                RowDefinitions =
                {                    
                    new RowDefinition{Height = new GridLength(4, GridUnitType.Star)},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)}
                },
                Padding = new Thickness(5, 5, 5, 5)
            };
            horizontalStacklayoutButtons.Children.Add(completeButton,0,0);
            horizontalStacklayoutButtons.Children.Add(getEntryButton,1,0);
            horizontalStacklayoutButtons.Children.Add(backButton, 2, 0);
            
            grid.Children.Add(listView, 0, 0);
            Grid.SetColumnSpan(listView, 2);
            grid.Children.Add(scanner, 1, 0);
            grid.Children.Add(horizontalStacklayoutButtons, 0, 1);
            Grid.SetColumnSpan(horizontalStacklayoutButtons, 2);

            return grid;
        }
       
        private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            bool ok = await DisplayAlert("!!!", "Удалить элемент?", "OK", "Cancel");
            if (ok)
            {
                var p = e.SelectedItem as Tool;
                ScannerTools.Remove(p);
                listView.ItemsSource = ScannerTools;
            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            scanner.IsScanning = true;
            scanner.IsVisible = false;
        }

        protected override void OnDisappearing()
        {
            scanner.IsScanning = false;
            base.OnDisappearing();
        }

        public static Task<string> InputBox(INavigation navigation)
        {           
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Title", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Enter new text:" };
            var txtInput = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {

                var result = txtInput.Text;
                await navigation.PopModalAsync();

                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {

                await navigation.PopModalAsync();

                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);

            txtInput.Focus();

            return tcs.Task;
        }

    }

}