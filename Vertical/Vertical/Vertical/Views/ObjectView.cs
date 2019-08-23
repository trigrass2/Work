using Acr.UserDialogs;
using Syncfusion.XForms.TextInputLayout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Vertical.CustomViews;
using Vertical.Models;
using Vertical.Services;
using Vertical.ViewModels;
using Vertical.Views.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Essentials.Controls;
using Kit = Plugin.InputKit.Shared.Controls;

namespace Vertical.Views
{
    /// <summary>
    /// Отображение объекта
    /// </summary>
	public class ObjectView : ContentView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CheckPageViewModel _viewModel { get; set; }

        private static string _fontFamily = "fontawesome-webfont.ttf#Material Design Icons";

        private StackLayout mainStack = new StackLayout { Padding = 5 };
        public ChecklistDataTemplateSelector templateSelector;

        public static BindableProperty ObjectGUIDProperty =
            BindableProperty.Create(
                propertyName: "ObjectGUID",
                returnType: typeof(SystemObjectPropertyValueModel),
                declaringType: typeof(CheckListView),
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

        public ObjectView()
        {
            BackgroundColor = Color.White;
        }

        public ObjectView(CheckPageViewModel vm)
        {
            BackgroundColor = Color.White;
            _viewModel = vm;
            BindingContext = _viewModel;
            Init();
        }

        private void Init()
        {
            CreateSelectors();
            SetBindings();
            stateContainer.Conditions[0].Content = mainStack;
            this.Content = stateContainer;
        }

        private void SetBindings()
        {
            stateContainer.SetBinding(StateContainer.StateProperty, "States");
            BindableLayout.SetItemsSource(mainStack, (BindingContext as CheckPageViewModel).Source.Result.DisplayItems);//если не работают кнопки то obs
            BindableLayout.SetItemTemplateSelector(mainStack, templateSelector);
        }

        private void UpdateUI()
        {
            Grid mainGrid = new Grid();
            //var items = (BindingContext as CheckPageViewModel).Source.Result.DisplayItems as Dictionary<int?, ObservableCollection<SystemObjectPropertyValueModel>>;
        }

        private StateContainer stateContainer = new StateContainer
        {
            Conditions = new List<StateCondition>
            {
                new StateCondition
                {
                   Is="Normal",
                   BackgroundColor = Color.White
                },
                new StateCondition
                {
                    Is="Loading",
                    Content = new ActivityIndicator
                    {
                        IsRunning = true,
                        Style = (Style)Xamarin.Forms.Application.Current.Resources["ActivityIndicatorStyle"]
                    },
                    BackgroundColor = Color.White
                },
                new StateCondition
                {

                    Is="NoData",
                    Content = new Label
                    {
                        Text = "Данных пока нет"
                    },
                    BackgroundColor = Color.White
                }
            }
        };

        #region DataTemplates

        private void CreateSelectors()
        {
            
            templateSelector = new ChecklistDataTemplateSelector
            {
                GroupTemplate = new DataTemplate(() => CreateGroupDataTemplate()),
                BoolTemplate = new DataTemplate(() => CreateBoolTemplate()),
                DateTimeTemplate = new DataTemplate(() => CreateDateTimeTemplate()),
                FloatTemplate = new DataTemplate(() => CreateFloatDataTemplate()),
                IntTemplate = new DataTemplate(() => CreateIntDataTemplate()),
                ArrayTemplate = new DataTemplate(() => CreateArrayDataTemplate()),
                HumanTemplate = new DataTemplate(() => CreateHumanDataTemplate()),
                NotArrayTemplate = new DataTemplate(() => CreateNotArrayDataTemplate()),
                GibridObjectTemplate = new DataTemplate(() => CreateGibridDataTemplate()),
                ObjectTemplate = new DataTemplate(() => CreateDefaultObjectView()),
                StringTemplate = new DataTemplate(() => CreateStringDataTemplate())
            };
        }

        
        public Label CreateGroupDataTemplate()
        {
            Label label = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };
            try
            {
                label.SetBinding(Label.TextProperty, "Key");
                label.SetBinding(Label.IsVisibleProperty, "Key", converter: new GroupNameVisibleConverter());
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
            }


            return label;
        }
        public Frame CreateBoolTemplate()
        {
            try
            {
                Frame frame = new Frame
                {
                    Padding = 1,
                    HasShadow = false,
                    BorderColor = (Color)Application.Current.Resources["LightGreyColor"],
                    CornerRadius = 0
                };

                Kit.CheckBox checkBox = new Kit.CheckBox
                {
                    Margin = new Thickness(5, 3),
                    BoxSizeRequest = 40,
                    Color = (Color)Application.Current.Resources["GreenColor"],
                    BorderColor = (Color)Application.Current.Resources["LightGreyColor"],
                    Type = Kit.CheckBox.CheckType.Material
                };
                checkBox.SetBinding(Kit.CheckBox.IsCheckedProperty, "Value");
                checkBox.SetBinding(Kit.CheckBox.CheckChangedCommandProperty, new Binding("BindingContext.IsCheckedCommand", source: this));
                checkBox.SetBinding(Kit.CheckBox.CommandParameterProperty, new Binding("BindingContext", source: frame));

                Label label = new Label
                {
                    TextColor = Color.Black,
                    FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label)),
                    Margin = 5
                };
                label.SetBinding(Label.TextProperty, "Name");

                frame.Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                {
                    label,
                    checkBox
                }
                };

                return frame;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(Frame);
            }

        }
        public SfTextInputLayout CreateDateTimeTemplate()
        {
            try
            {
                DatePicker datePicker = new DatePicker
                {
                    Format = "dd.MM.yyyy",
                    FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(DatePicker)),
                    TextColor = Color.Black
                };
                datePicker.SetBinding(DatePicker.MinimumDateProperty, new Binding("BindingCintext.MinDate", source: this));
                datePicker.SetBinding(DatePicker.MaximumDateProperty, new Binding("BindingCintext.MaxDate", source: this));
                datePicker.SetBinding(DatePicker.DateProperty, "Value");
                datePicker.DateSelected += DatePicker_DateSelected;

                SfTextInputLayout inputLayout = new SfTextInputLayout
                {
                    Style = Application.Current.Resources["SfTextInputLayoutStyle"] as Style,
                    Margin = new Thickness(0, 0, 0, 10),
                    InputView = datePicker
                };
                inputLayout.SetBinding(SfTextInputLayout.BindingContextProperty, new Binding("BindingContext", source:Parent));
                inputLayout.SetBinding(SfTextInputLayout.HelperTextProperty, "Name");
                inputLayout.SetBinding(SfTextInputLayout.IsEnabledProperty, "Locked");

                return inputLayout;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(SfTextInputLayout);
            }
        }
        public SfTextInputLayout CreateFloatDataTemplate()
        {
            try
            {
                Entry entry = new Entry
                {
                    FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Entry)),
                    TextColor = Color.Black
                };

                
                SfTextInputLayout inputLayout = new SfTextInputLayout
                {
                    Style = (Style)Application.Current.Resources["SfTextInputLayoutStyle"],
                    Margin = new Thickness(0, 0, 0, 10),
                    InputView = entry
                };
                inputLayout.SetBinding(SfTextInputLayout.HelperTextProperty, "Name");
                inputLayout.SetBinding(SfTextInputLayout.IsEnabledProperty, "Locked");

                entry.SetBinding(Entry.TextProperty, "Value");
                entry.SetBinding(Entry.BindingContextProperty, new Binding("BindingContext", source:inputLayout));
                entry.Completed += Entry_Completed_float;

                return inputLayout;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(SfTextInputLayout);
            }

        }
        public SfTextInputLayout CreateIntDataTemplate()
        {
            try
            {
                Entry entry = new Entry
                {
                    FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Entry)),
                    TextColor = Color.Black
                };

                SfTextInputLayout inputLayout = new SfTextInputLayout
                {
                    Style = (Style)Application.Current.Resources["SfTextInputLayoutStyle"],
                    Margin = new Thickness(0, 0, 0, 10),
                    InputView = entry
                };
                inputLayout.SetBinding(SfTextInputLayout.HelperTextProperty, "Name");
                inputLayout.SetBinding(SfTextInputLayout.IsEnabledProperty, "Locked");

                entry.SetBinding(Entry.TextProperty, "Value");
                entry.SetBinding(Entry.BindingContextProperty, new Binding("BindingContext", source: inputLayout));
                entry.Completed += Entry_Completed_int;
                
                return inputLayout;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(SfTextInputLayout);
            }
        }
        public Frame CreateArrayDataTemplate()
        {
            try
            {
                Frame frame = new Frame
                {
                    Padding = 0,
                    BorderColor = (Color)Application.Current.Resources["LightGreyColor"],
                    BackgroundColor = Color.White,
                    
                };

                frame.SetBinding(Frame.BindingContextProperty, new Binding("BindingContext", source:Parent));
                Grid grid = new Grid
                {
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition{ Height = 25 },
                        new RowDefinition{ Height = GridLength.Auto }
                    },
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition(),
                        new ColumnDefinition(){ Width=40 }
                    }
                };

                StackLayout stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0
                };
                grid.Children.Add(stackLayout, 0, 0);
                Grid.SetColumnSpan(stackLayout, 2);

                Label label = new Label
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    TextColor = Color.Black,
                    VerticalTextAlignment = TextAlignment.Center
                };
                label.SetBinding(Label.TextProperty, "Name");
                stackLayout.Children.Add(label);

                Button buttonAdd = new Button
                {
                    Text = "+",
                    TextColor = Color.FromHex("#555"),
                    FontAttributes = FontAttributes.Bold,
                    BackgroundColor = (Color)Application.Current.Resources["ControlsBackgrounColor"],
                    Padding = 0,
                    WidthRequest = 25
                };
                buttonAdd.SetBinding(Button.CommandProperty, new Binding("BindingContext.AddNewObjectInPropertyCommand", source: this));
                buttonAdd.SetBinding(Button.CommandParameterProperty, new Binding("BindingContext", source: grid));
                buttonAdd.SetBinding(Button.IsVisibleProperty, "Locked");
                stackLayout.Children.Add(buttonAdd);

                Button buttonDelete = new Button
                {
                    FontFamily = _fontFamily,
                    Text = (string)Application.Current.Resources["IconTrash"],
                    TextColor = Color.FromHex("#555"),
                    BackgroundColor = Color.Transparent,
                    Padding = 0
                };
                buttonDelete.SetBinding(Button.CommandProperty, new Binding("BindingContext.DeletePropertyCommand", source: this));
                buttonDelete.SetBinding(Button.CommandParameterProperty, new Binding("BindingContext", source: grid));
                buttonDelete.SetBinding(Button.IsVisibleProperty, "Locked");
                grid.Children.Add(buttonDelete, 1, 0);
                
                ObjectView objectView = new ObjectView
                {                    
                    MinimumHeightRequest = 80,
                    BackgroundColor = Color.Transparent
                };
                objectView.SetBinding(ObjectView.ObjectGUIDProperty, new Binding("BindingContext", source:frame));
                grid.Children.Add(objectView, 0, 1);
                Grid.SetColumnSpan(objectView, 2);

                frame.Content = grid;
                return frame;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(Frame);
            }
        }
        public SfTextInputLayout CreateStringDataTemplate()
        {
            try
            {
                Entry entry = new Entry
                {
                    FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Entry)),
                    TextColor = Color.Black
                };

                SfTextInputLayout inputLayout = new SfTextInputLayout
                {
                    Style = (Style)Application.Current.Resources["SfTextInputLayoutStyle"],
                    Margin = new Thickness(0, 0, 0, 10),
                    InputView = entry
                };
                inputLayout.SetBinding(SfTextInputLayout.HelperTextProperty, "Name");
                inputLayout.SetBinding(SfTextInputLayout.IsEnabledProperty, "Locked");

                entry.SetBinding(Entry.TextProperty, "Value");
                entry.SetBinding(Entry.BindingContextProperty, new Binding("BindingContext", source: inputLayout));
                entry.Completed += Entry_Completed_string;

                return inputLayout;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(SfTextInputLayout);
            }
        }
        public SfTextInputLayout CreateHumanDataTemplate()
        {
            try
            {
                Entry entry = new Entry
                {
                    FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Entry)),
                    TextColor = Color.Black
                };
               
                entry.SetBinding(Entry.TextProperty, "Value");

                SfTextInputLayout inputLayout = new SfTextInputLayout
                {
                    Style = (Style)Application.Current.Resources["SfTextInputLayoutStyle"],
                    Margin = new Thickness(0, 0, 0, 10),
                    InputView = entry
                };
                inputLayout.SetBinding(SfTextInputLayout.HelperTextProperty, "Name");
                inputLayout.SetBinding(SfTextInputLayout.IsEnabledProperty, "Locked");

                return inputLayout;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(SfTextInputLayout);
            }

        }
        public Label CreateDefaultObjectView()
        {
            try
            {
                Label label = new Label();
                label.SetBinding(Label.TextProperty, "Name");

                return label;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(Label);
            }
        }

        Button buttonPencil = new Button
        {
            FontFamily = _fontFamily,
            Text = (string)Application.Current.Resources["IconPencil"],
            TextColor = (Color)Application.Current.Resources["LightGreyColor"],
            BackgroundColor = Color.Transparent,
            Padding = 0
        };

        Label labelObject = new Label();
        private Frame CreateNotArrayDataTemplate()
        {
            try
            {
                labelObject.SetBinding(Label.TextProperty, "Value", converter: new ObjectConverter());
                buttonPencil.SetBinding(Button.CommandProperty, new Binding("BindingContext.EditObjectCommand", source: this));
                buttonPencil.SetBinding(Button.CommandParameterProperty, new Binding("BindingContext", source: buttonPencil?.Parent));
                buttonPencil.SetBinding(Button.IsVisibleProperty, "Locked");

                return CreateNestedObjectView(labelObject, buttonPencil);

            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(Frame);
            }

        }
        private Frame CreateGibridDataTemplate()
        {
            try
            {
                labelObject.SetBinding(Label.TextProperty, "Value", converter: new ObjectConverter());

                ObjectView objectView = new ObjectView
                {
                    MinimumHeightRequest = 80
                };
                objectView.SetBinding(ObjectView.ObjectGUIDProperty, new Binding("BindingContext", source: Parent));
                var frame = CreateNestedObjectView(new StackLayout { Children = { labelObject, objectView } }, buttonPencil);

                return frame;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(Frame);
            }

        }
        private Frame CreateNestedObjectView(View view, Button button)
        {
            try
            {
                Frame frame = new Frame
                {
                    Padding = 0,
                    BorderColor = (Color)Application.Current.Resources["LightGreyColor"],
                    BackgroundColor = Color.White
                };

                Grid grid = new Grid
                {
                    Margin = 5,
                    RowSpacing = 0,
                    RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 25 },
                    new RowDefinition{ Height = GridLength.Auto }
                }
                };

                StackLayout stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0
                };
                grid.Children.Add(stackLayout, 0, 0);

                Label label = new Label
                {
                    TextColor = Color.Black,
                    VerticalTextAlignment = TextAlignment.Center
                };
                label.SetBinding(Label.TextProperty, "Name");
                stackLayout.Children.Add(label);

                stackLayout.Children.Add(button);
                grid.Children.Add(view, 0, 1);

                frame.Content = grid;
                return frame;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(Android.Util.LogPriority.Error, ex.Message);
                return default(Frame);
            }

        }
        #endregion

        #region Actoins

        private async void Entry_Completed_float(object sender, EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            if (double.TryParse(model.Value as string, out double d) == false && !(model.Value is null) && model.Value as string != "")
            {
                await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.TypeName}", "Ошибка", "Ок");
                return;
            }

            _viewModel.CreateNewValue(model, model.Value);
        }

        private async void Entry_Completed_int(object sender, EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            if (int.TryParse(model.Value as string, out int i) == false && !(model.Value is null) && model.Value as string != "")
            {
                await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.TypeName}", "Ошибка", "Ок");
                return;
            }

            _viewModel.CreateNewValue(model, model.Value);
        }

        
        private void Entry_Completed_string(object sender, EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            _viewModel.CreateNewValue(model, model.Value);
        }
        bool firstTime = false;

        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if(firstTime)
            if(e.OldDate != e.NewDate)
            {                
               await _viewModel?.Savedate((sender as DatePicker).BindingContext as SystemObjectPropertyValueModel);
            }
            firstTime = true;
        }
        #endregion

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == ObjectGUIDProperty.PropertyName)
            {
                var item = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = ObjectGUID.Value }).FirstOrDefault();
                _viewModel = new CheckPageViewModel(item) { Navigation = this.Navigation };
                BindingContext = _viewModel;
                Init();
            }
            //else if (propertyName == ArrayValuesProperty.PropertyName)
            //{
            //    _viewModel = new CheckPageViewModel(ArrayValues.ArrayValue, ArrayValues.SystemObjectGUID, ArrayValues.ID) { Navigation = this.Navigation };
            //}
        }
    }
}