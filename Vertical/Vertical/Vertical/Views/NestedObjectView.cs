using Syncfusion.XForms.Buttons;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NestedObjectView : ContentView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static BindableProperty ObjectGUIDProperty =
            BindableProperty.Create(
                propertyName: "ObjectGUID",
                returnType: typeof(SystemObjectPropertyValueModel),
                declaringType: typeof(NestedObjectView),
                defaultValue: default(string),
                defaultBindingMode: BindingMode.TwoWay
                );

        public SystemObjectPropertyValueModel ObjectGUID
        {
            get
            {
                return (SystemObjectPropertyValueModel)GetValue(ObjectGUIDProperty);
            }
            set
            {
                SetValue(ObjectGUIDProperty, value);
            }
        }
        public CheckPageViewModel ViewModel { get; set; }

        public StackLayout MainStack;
        public SfCheckBox sfCheckBox;
        public NestedObjectView(CheckPageViewModel viewModel)
        {
            ViewModel = viewModel;
            BackgroundColor = Color.LightGray;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == ObjectGUIDProperty.PropertyName)
            {
                Content = new StackLayout
                {
                    
                };
            }
        }

        private void SetContent()
        {
            var items = ViewModel.SystemPropertyModels;

        }
        
    }
}