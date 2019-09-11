using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.Models;
using Vertical.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Essentials.Controls;
using System.Collections.Generic;
using System;
using Vertical.Services;
using Acr.UserDialogs;

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
                        
            mainGrid.Children.Add(savePropertiesButton, 0, 3);
            Grid.SetColumnSpan(savePropertiesButton, 2);

            #endregion region

            Content = new ScrollView() { Content = mainGrid };

            savePropertiesButton.Clicked += async (sender, e) =>
            {
                using (UserDialogs.Instance.Loading("Сохранение изменений...", null, null, true, MaskType.Black))
                {
                    await SaveChanges((CheckListView)mainGrid.Children.FirstOrDefault(x => x.GetType() == typeof(CheckListView)));
                    await Navigation.PopAsync();
                }
                
            };
        }
        
        private async void ContextMenuButton_Clicked(object sender, EventArgs e)
        {            
            await DisplayActionSheet(null, null, null,"Копировать","Удалить");
        }
        private async Task SaveChanges(CheckListView viewObj)
        {
            try
            {
                if (viewObj?.ViewModel == null) return;
                await (viewObj.BindingContext as CheckPageViewModel).SavePropertiesValuesAsync();
                var vs = ((viewObj.Children.FirstOrDefault(x => x.GetType() == typeof(StateContainer)) as StateContainer).Conditions[0].Content as StackLayout).Children;

                if (vs == null)
                {
                    return;
                }
                foreach (var view in vs.AsParallel())
                {
                    await GetView(view);
                }
            }
            catch (Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
            }

        }

        private async Task GetView(View v)
        {
            try
            {
                switch (v.GetType().Name)
                {
                    case "StackLayout":
                        {
                            foreach (var s in (v as StackLayout).Children.AsParallel())
                            {
                                await GetView(s);
                            }
                        }; break;

                    case "Grid":
                        {
                            foreach (var s in (v as Grid).Children.AsParallel())
                            {
                                await GetView(s);
                            }
                        }; break;

                    case "CheckListView":
                        {
                            await SaveChanges(v as CheckListView);
                        }; break;

                    default: return;
                }
            }
            catch (Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
            }
            
            
        }
        
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
            IsVisible = false,
            BackgroundColor = Color.White,
            CornerRadius = 0,
            Margin = new Thickness(0, 0, 0, 0.5)
        };
    }
}