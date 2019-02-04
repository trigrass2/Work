using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Syncfusion.ListView.XForms;
using Syncfusion.DataSource;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using ZXing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AddingProductUnit
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<ProductUnit> ProductUnits { get; set; }
        public List<ProductRegion> Regions { get; set; }
        private SfListView listView;
        StackLayout mainStackLayout = new StackLayout();        
        private string cacheFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.bin");

        public MainPage()
        {            
            InitializeComponent();

            #region base
            
            ProductUnits = RestApi.GetProductUnitList();

            var title = new Label()
            {
                Text = "Участки производства",
                FontFamily = "Arial",
                FontSize = 25,
                TextColor = Color.White,
                Margin = new Thickness(5, 0, 0, 0)
            };

            listView = new SfListView()
            {                
                ItemsSource = ProductUnits,
                ItemSpacing = new Thickness(5, 5, 5, 5),               
                
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Button {
                        BackgroundColor = Color.FromHex("#5990c6"),
                        BorderWidth = 1,
                        TextColor = Color.White,                                              
                        FontAttributes = FontAttributes.Bold,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                        FontFamily = "Arial",
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        CornerRadius = 7
                        
                    };
                    label.SetBinding(Button.TextProperty, "Name");
                   
                    return label;
                })

            };

            listView.SelectionChanging += ListView_SelectionChanging;           

            mainStackLayout.Children.Add(title);
            mainStackLayout.Children.Add(listView);
           
            Content = mainStackLayout;
            #endregion
            
            
        }

        private async void ListView_SelectionChanging(object sender, ItemSelectionChangingEventArgs e)
        {
            ProductUnit needItem = (ProductUnit)e.AddedItems[0];
            
            if (e.AddedItems.Count > 0 && e.AddedItems[0] == ProductUnits[0])
                e.Cancel = true;

            var regionPageContent = GetRegionPageContent(needItem.Unit_id);
            await Navigation.PushAsync(new RegionListPage(regionPageContent));
        }        

        private List<ProductRegion> GetRegionPageContent(int unitId)
        {
            if ((File.Exists(cacheFile) == true && File.GetLastWriteTime(cacheFile).Day < DateTime.Now.Day) || !File.Exists(cacheFile))
            {
                Regions = RestApi.GetProductRegionList();                
                SerializeObject(Regions, cacheFile);
            }
            else
            {
                Regions = DeserializeObject(cacheFile);
            }

            return Regions.FindAll(x => x.Unit_id == unitId); 
        }

        private List<ProductRegion> DeserializeObject(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return default(List<ProductRegion>); }
            if (!File.Exists(fileName)) { return default(List<ProductRegion>); }
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter binForm = new BinaryFormatter();
                fs.Seek(0, SeekOrigin.Begin);
                List<ProductRegion> obj = (List<ProductRegion>)binForm.Deserialize(fs);
                return obj;
            }
        }

        private void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, serializableObject);

            }
        }
    }      
}
