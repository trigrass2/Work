using System.ComponentModel;
using System.Runtime.CompilerServices;
using Vertical.Models;
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

        public NestedObjectView()
        {
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

        

        //public StateContainer stateContainer = new StateContainer
        //{
        //    State = _states,
        //    Conditions =
        //    {
        //        new StateCondition
        //        {
        //            Is = "Loading",
        //            Content = new ActivityIndicator
        //            {
        //                IsRunning = true, 
        //                Color = Color.FromHex("#ff4114"),
        //                HeightRequest = 50,
        //                WidthRequest = 50,
        //                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true),
        //                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true)
        //            }
        //        },
        //        new StateCondition
        //        {
        //            Is = "Normal",
        //            Content = new StackLayout
        //            {
        //                Children =
        //                {
        //                    new Label
        //                    {
        //                        Text = ObjectGUID.Name,
        //                        FontSize = 12
        //                    },
        //                    new CheckListView()
        //                    {
        //                        ObjectGUID = ObjectGUID
        //                    },
        //                    new Button {
        //                        Text = "добавить объект",
        //                        TextColor = Color.Black,                                
        //                        FontSize = 10,
        //                        BackgroundColor = Color.FromHex("#43B05C"),
        //                        CornerRadius = 5,
        //                        WidthRequest = 100,
        //                        Padding = new Thickness(1)                                
        //                    }
        //                }
        //            }
        //        }
        //    }
        //};
    }
}