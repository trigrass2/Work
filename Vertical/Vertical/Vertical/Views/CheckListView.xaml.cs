using Acr.UserDialogs;
using System;
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

        public bool IsChange { get; set; } = false;
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

        
        public CheckListView (CheckPageViewModel viewModel)
		{
			InitializeComponent ();
            ViewModel = viewModel;
            BindingContext = ViewModel;            
        }

        #region Events

        private async void Entry_TextChanged_int(object sender, TextChangedEventArgs e)
        {
            try
            {
                int? oldV;
                int? newV;

                if (int.TryParse(e.OldTextValue, out int o))
                {
                    oldV = o;
                }
                else oldV = null;

                if (int.TryParse(e.NewTextValue, out int n))
                {
                    newV = n;
                }
                else newV = null;

                await SaveChange<int?>((sender as Entry).BindingContext as SystemObjectPropertyValueModel, oldV, newV);
            }
            catch (Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
            }
            
        }

        private async void Entry_TextChanged_string(object sender, TextChangedEventArgs e)
        {
            try
            {
                await SaveChange<string>((sender as Entry).BindingContext as SystemObjectPropertyValueModel, e.OldTextValue, e.NewTextValue);
            }
            catch (Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
            }
            
        }

        private async void Entry_TextChanged_float(object sender, TextChangedEventArgs e)
        {
            try
            {
                double? oldV;
                double? newV;

                if (double.TryParse(e.OldTextValue, out double o))
                {
                    oldV = o;
                }
                else oldV = null;

                if (double.TryParse(e.NewTextValue, out double n))
                {
                    newV = n;
                }
                else newV = null;

                await SaveChange<double?>((sender as Entry).BindingContext as SystemObjectPropertyValueModel, oldV, newV);
            }
            catch (Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
            }
            
        }
                
        private async Task SaveChange<T>(SystemObjectPropertyValueModel model, object oldValue, object newValue)
        {
            
            if (oldValue != newValue)
            {
                
                if (string.IsNullOrEmpty(newValue?.ToString()) || newValue is T)
                {
                    ViewModel.CreateNewValue(model, newValue);
                    IsChange = true;
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.TypeName}", "Ошибка", "Ок");
                    return;
                }
            }
        }

        bool firstTime = false;
        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            //if (firstTime)
                if (e?.OldDate != e?.NewDate)
                {
                    await ViewModel?.SaveDate((sender as DatePicker).BindingContext as SystemObjectPropertyValueModel);
                    IsChange = true;
                }
            //firstTime = true;
        }

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
    }
}