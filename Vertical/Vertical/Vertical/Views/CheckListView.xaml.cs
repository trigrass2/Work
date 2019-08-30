using Acr.UserDialogs;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Vertical.Models;
using Vertical.Services;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckListView : ContentView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private int _entryValueInt { get; set; }
        private double _entryValueDouble { get; set; }
        private string _entryValueString { get; set; }

        public CheckPageViewModel ViewModel { get; set; }

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

        public CheckListView()
        {
            InitializeComponent();
        } 

        #region Events
        public CheckListView (CheckPageViewModel viewModel)
		{
			InitializeComponent ();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private async void Entry_TextChanged_int(object sender, TextChangedEventArgs e)
        {
             await SaveChange<int>((sender as Entry).BindingContext as SystemObjectPropertyValueModel, e.OldTextValue, e.NewTextValue);
        }

        private async void Entry_TextChanged_string(object sender, TextChangedEventArgs e)
        {
             await SaveChange<string>((sender as Entry).BindingContext as SystemObjectPropertyValueModel, e.OldTextValue, e.NewTextValue);
        }

        private async void Entry_TextChanged_float(object sender, TextChangedEventArgs e)
        {
             await SaveChange<double>((sender as Entry).BindingContext as SystemObjectPropertyValueModel, e.OldTextValue, e.NewTextValue);
        }

        bool firstTime = false;
        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (firstTime)
                if (e.OldDate != e.NewDate)
                {
                    await ViewModel?.SaveDate((sender as DatePicker).BindingContext as SystemObjectPropertyValueModel);
                }
            firstTime = true;
        }        

        private async Task SaveChange<T>(SystemObjectPropertyValueModel model, string oldValue, object newValue)
        {
            if (oldValue != null && oldValue != newValue as string)
            {
                if (newValue is T)
                {
                    //ViewModel.CreateNewValue(model, newValue);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.TypeName}", "Ошибка", "Ок");
                    return;
                }
            }
            
            

        }
        //private async void Entry_Completed_string(object sender, EventArgs e)
        //{            
        //    //var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
        //    //ViewModel.CreateNewValue(model, model.Value);
        //}

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == ObjectGUIDProperty.PropertyName)
            {
                if(!string.IsNullOrEmpty(ObjectGUID.Value as string))
                {
                    var item = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = ObjectGUID.Value, ShowHidden = true }).FirstOrDefault();
                    ViewModel = new CheckPageViewModel(item) { Navigation = this.Navigation };
                    BindingContext = ViewModel;
                }
                
            }
        }



        #endregion

        private void Entry_Unfocused_string(object sender, FocusEventArgs e)
        {
            ViewModel.CreateNewValue((sender as Entry).BindingContext as SystemObjectPropertyValueModel, _entryValueString);
        }

        private void AdvancedEntry_Completed(object sender, EventArgs e)
        {
            var model = (sender as AdvancedEntry).BindingContext as SystemObjectPropertyValueModel;
            ViewModel.CreateNewValue(model, model.Value);
        }
    }
}