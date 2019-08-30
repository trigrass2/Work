using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.Models;
using Vertical.ViewModels;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckListPage : ContentPage
	{        
        public CheckPageViewModel ViewModel { get; set; }
        
        public CheckListPage(SystemObjectModel systemObjectModel)
		{
			InitializeComponent ();

            ViewModel = new CheckPageViewModel(systemObjectModel) { Navigation = this.Navigation };
            contextMenuButton.Clicked += ContextMenuButton_Clicked;
            #region temp
            headerGrid.Children.Add(new Label
            {
                Text = "Информация",
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                FontAttributes = FontAttributes.Bold
            },0,0);

            headerGrid.Children.Add(contextMenuButton, 1,0);

            headerGrid.Children.Add(new Label
            {
                Text = systemObjectModel.Name,
                FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label))
            },0,1);

            headerGrid.Children.Add(new Label
            {
                Text = systemObjectModel.TypeName,
                FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label))
            },0,2);

            headerGrid.Children.Add(new Label
            {
                Text = systemObjectModel.CreationTime.ToString("yyyy-MM-dd HH:MM"),
                FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label))
            },0,3);

            headerGrid.Children.Add(new Label
            {
                Text = systemObjectModel.UserName,
                FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label))
            },0,4);
            
            mainGrid.Children.Add(headerGrid, 0, 0);
            Grid.SetColumnSpan(headerGrid, 2);
            var label = new Label
            {
                Text = " Параметры",
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.White
            };

            mainGrid.Children.Add(label, 0,1);
            Grid.SetColumnSpan(label, 2);

            var view = new CheckListView(ViewModel);//new ObjectView(ViewModel);
            mainGrid.Children.Add(view, 0, 2);
            Grid.SetColumnSpan(view, 2);

            savePropertiesButton.BindingContext = ViewModel;
            savePropertiesButton.Command = ViewModel.SavePropertiesValuesCommand;                      

            //mainGrid.Children.Add(new ScrollView() { Content = mainGrid }, 0, 0);
            mainGrid.Children.Add(savePropertiesButton, 0, 3);
            Grid.SetColumnSpan(savePropertiesButton, 2);
            #endregion region

            Content = new ScrollView() { Content = mainGrid };
        }

        private async void ContextMenuButton_Clicked(object sender, System.EventArgs e)
        {
            await DisplayActionSheet(null, null, null,"Копировать","Удалить");
        }

        //public Grid backGroundGrid = new Grid
        //{            
        //    RowDefinitions = new RowDefinitionCollection
        //    {
        //        new RowDefinition(),
        //        new RowDefinition(){ Height = GridLength.Auto }
        //    },
        //    RowSpacing = 0,
        //    BackgroundColor = Color.FromRgba(0,0,0,0)
        //};

        public Grid mainGrid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(){ Height = GridLength.Auto},
                new RowDefinition(){ Height = GridLength.Auto},
                new RowDefinition(),
                new RowDefinition(){ Height = 50 }
            },
            BackgroundColor = Color.FromHex("#6c757d"),
            RowSpacing = 0,
            ColumnSpacing = 0
        };

        public Grid headerGrid = new Grid
        {
            Padding = new Thickness(5, 0, 0, 5),
            BackgroundColor = Color.White,
            Margin = new Thickness(0, 0, 0, 0.5),
            RowSpacing = 0, ColumnSpacing = 0,
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition{ Height=25 },
                new RowDefinition{ Height=25 },
                new RowDefinition{ Height=25 },
                new RowDefinition{ Height=25 },
                new RowDefinition{ Height=25 }
            },
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(),
                new ColumnDefinition(){ Width = 35}
            }
        };

        public Button savePropertiesButton = new Button
        {
            BackgroundColor = Color.FromHex("#3cb371"),
            HeightRequest = 50,            
            Text = "OK",
            TextColor = Color.White,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 0.5, 0, 0.5)
        };

        public Button contextMenuButton = new Button
        {
            AnchorX = 0, AnchorY = 0,
            ImageSource = new FontImageSource()
            {
                Glyph = "\uf141",
                FontFamily = "fontawesome-webfont.ttf#Material Design Icons",
                Color = Color.FromHex("#ccc"),
                Size = 17
            },
            BackgroundColor = Color.White,
            CornerRadius = 0,
            Margin = new Thickness(0, 0, 0, 0.5)
        };
    }
}