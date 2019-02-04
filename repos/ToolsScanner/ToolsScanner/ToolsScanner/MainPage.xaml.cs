using System;
using System.Collections.ObjectModel;
using System.Linq;
using ToolsScanner.Model;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolsScanner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        
        public ListView listViewTools;
        public ObservableCollection<Person> Persons { get; set; }
       
        public ObservableCollection<Tool> SourceTools { get;set; }
        public ObservableCollection<Tool> Tools { get; set; } 

        public Person PersonName { get; set; }

        public MainPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);

            scanPersonButton.Clicked += ScanPersonButtonClick;
            scanToolsButton.Clicked += ScanToolsButtonClick;

            PersonName = new Person();
            Persons = new ObservableCollection<Person>(RestApi.GetPersonList());
            SourceTools = new ObservableCollection<Tool>(RestApi.GetToolsList());
            Tools = new ObservableCollection<Tool>();
            stackLayoutButtons.Children.Add(scanPersonButton);
            stackLayoutButtons.Children.Add(scanToolsButton);

            listViewTools = GetListViewContent(Tools);            
            
            mainGrid.Children.Add(titlePersonLabel, 0, 0);
            mainGrid.Children.Add(listViewTools, 0, 1);
            mainGrid.Children.Add(stackLayoutButtons, 0, 2);                     

            Content = mainGrid;
        }

        private ObservableCollection<Tool> UpdateTools(string personID)
        {
            Tools.Clear();
            foreach(var t in SourceTools.Where(x => x.Person_id == personID))
            {
                Tools.Add(t);
            }
            return Tools;
        }
        Grid mainGrid = new Grid
        {
            RowDefinitions = 
            {
                new RowDefinition{Height = new GridLength(1, GridUnitType.Star)},
                new RowDefinition{Height = new GridLength(3, GridUnitType.Star)},
                new RowDefinition{Height = new GridLength(1, GridUnitType.Star)}
            }
        };        

        Label titlePersonLabel = new Label
        {              
            TextColor = Color.FromHex("#ffeeed"),
            FontFamily = "Verdana",
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            
            Margin = new Thickness(5, 5, 5, 5)            
        };

        StackLayout stackLayoutButtons = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 3
        };

        Button scanPersonButton = new Button
        {
            Text = "Person",
            TextColor = Color.FromHex("#464451"),
            FontFamily = "Verdana",
            BackgroundColor = Color.FromHex("#ffeeed"),
            Margin = new Thickness(5,5,0,5),
            CornerRadius = 7,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        Button scanToolsButton = new Button
        {
            Text = "Tools",
            TextColor = Color.FromHex("#464451"),
            FontFamily = "Verdana",
            BackgroundColor = Color.FromHex("#ffeeed"),
            Margin = new Thickness(5, 5, 5, 5),
            CornerRadius = 7,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        private void ScanToolsButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Navigation.PushAsync(new ScanPage(SourceTools));           
        }

        private void ScanPersonButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Navigation.PushAsync(new ScanPage(Persons));
        }

        private ListView GetListViewContent(ObservableCollection<Tool> toolsOfPerson)
        {           

            ListView listView = new ListView
            {
                SeparatorColor = Color.Gray,
                HasUnevenRows = true,
                ItemsSource = Tools,                
                ItemTemplate = new DataTemplate(() => {
                    Label labelNameTool = new Label
                    {
                        TextColor = Color.FromHex("#ffeeed"),
                        FontFamily = "Verdana",
                        FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label))
                    };
                    labelNameTool.SetBinding(Label.TextProperty, "Tool_name");

                    Label idTool = new Label
                    {
                        TextColor = Color.FromHex("#f2d9dd"),
                        FontFamily = "Verdana",
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    };
                    idTool.SetBinding(Label.TextProperty, "Tool_id");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { labelNameTool, idTool }
                        }
                    };
                })
            };
            return listView;
        }
        
        protected async internal void AddTools(ObservableCollection<Tool> barcodes)
        {
            if(barcodes != null && barcodes.Count != 0)
            {               
                Tools = barcodes;
                listViewTools.ItemsSource = Tools;

                if (PersonName.Person_id != null && Tools.Any(x => x.Person_id != PersonName.Person_id))
                {
                    bool ok = await DisplayAlert("!", "Присвоить инструменты?", "OK", "Cancel");
                    if (ok)
                    {
                        string status = null;
                        foreach (var t in Tools)
                        {                            
                            if (t.Person_name != PersonName.Person_name)
                            {
                                status = RestApi.ChangeHolder(PersonName.Person_id, t.Tool_id);                                
                            }                            
                        }
                        if (status != null)
                        Message(status);
                        listViewTools.ItemsSource = UpdateTools(PersonName.Person_id);
                    }
                }
            }
        }

        protected async internal void AddPerson(Person person)
        {
            if (person != null)
            {
                PersonName = person;
                titlePersonLabel.Text = person.Person_name;
                listViewTools.ItemsSource = UpdateTools(PersonName.Person_id);
                if (Tools.Count > 0 && Tools.Any(x => x.Person_id != PersonName.Person_id))
                {
                    bool ok = await DisplayAlert("!", "Присвоить инструмент?", "OK", "Cancel");
                    if (ok)
                    {
                        string status = null;
                        foreach (var t in Tools)
                        {                            
                            if (t.Person_name != PersonName.Person_name)
                            {
                                status = RestApi.ChangeHolder(PersonName.Person_id, t.Tool_id);                                
                            }                            
                        }
                        if(status != null)
                        Message(status);
                        listViewTools.ItemsSource = UpdateTools(PersonName.Person_id);
                    }
                }
            }            
        }

        private void Message(string status)
        {
            DisplayAlert("Статус отправки", status, "OK");
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}