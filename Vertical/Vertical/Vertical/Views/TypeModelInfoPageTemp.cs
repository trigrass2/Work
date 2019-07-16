using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vertical.ViewModels;
using Xamarin.Forms;
using Syncfusion.XForms.TextInputLayout;
using Syncfusion.Buttons.XForms;

namespace Vertical.Views
{
    public class TypeModelInfoPageTemp : ContentPage
    {
        TypeModelInfoPageViewModel _viewModel;        
        
        List<Label> _properties;
        Binding _titlelabelBinding;
        Binding _listViewPropertiesBinding;

        public TypeModelInfoPageTemp()
        {
            //_viewModel = new TypeModelInfoPageViewModel { Navigation = this.Navigation };
            _titlelabelBinding = new Binding { Source = _viewModel.SelectedSystemObjectTypeModel, Path = "Name" };
            _titlelabel.SetBinding(Label.TextProperty, _titlelabelBinding);
            _listViewPropertiesBinding = new Binding { Source = _viewModel.SystemPropertyModels, Path = "Name" };

            Content = new StackLayout
            {
                Children = {
                    _titlelabel,                    
                }
            };
        }
        
        private void UpdateContent()
        {
            foreach(var s in _viewModel.SystemPropertyModels)
            {
                
            }
        }

        Label _titlelabel = new Label
        {
            FontSize = 15
        };

        ListView _listViewProperties = new ListView(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
            SeparatorVisibility = SeparatorVisibility.None,
            HasUnevenRows = true,
            Margin = new Thickness(10),
            Header = "Привязанные свойства"            
        };

        Button _addNewProperties = new Button
        {
            Text = "привязать новое свойство",
            BackgroundColor = Color.FromHex("#336DAB"),
            TextColor = Color.White,
            CornerRadius = 5
        };
    }
}