using Acr.UserDialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private async void Entry_Completed_float(object sender, EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            if (double.TryParse(model.Value as string, out double d) == false && !(model.Value is null) && model.Value as string != "")
            {
                await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.Typename}", "Ошибка", "Ок");
                return;
            }

            ViewModel.CreateNewValue(model, model.Value);

        }

        private async void Entry_Completed_int(object sender, EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            if (int.TryParse(model.Value as string, out int i) == false && !(model.Value is null) &&  model.Value as string != "")
            {
                await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.Typename}", "Ошибка", "Ок");
                return;
            }

            ViewModel.CreateNewValue(model, model.Value);

        }

        private void Entry_Completed_string(object sender, EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            ViewModel.CreateNewValue(model, model.Value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == ObjectGUIDProperty.PropertyName)
            {
                var item = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = ObjectGUID.Value }).FirstOrDefault();
                ViewModel = new CheckPageViewModel(item) { Navigation = this.Navigation };
                BindingContext = ViewModel;
            }
        }
        #endregion

    }
}