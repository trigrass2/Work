using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.Models;
using Vertical.ViewModels;
using System.Linq;
using Syncfusion.XForms.Buttons;
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
            BindingContext = ViewModel;
		}

        private void SfCheckBox_StateChanged(object sender, StateChangedEventArgs e)
        {
            var model = (sender as SfCheckBox).BindingContext as SystemObjectPropertyValueModel;
            ViewModel.CreateNewValue(model, e.IsChecked);           
        }

        private async void Entry_Completed_float(object sender, System.EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            if(model.Value is double == false)
            {
                await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.Typename}", "Ошибка", "Ок");
                return;
            }
            ViewModel.CreateNewValue(model, model.Value);
        }

        private async void Entry_Completed_int(object sender, System.EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            if (model.Value is int == false)
            {
                await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.Typename}", "Ошибка", "Ок");
                return;
            }
            ViewModel.CreateNewValue(model, model.Value);
        }
        
        private void Entry_Completed_string(object sender, System.EventArgs e)
        {
            var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
            ViewModel.CreateNewValue(model, model.Value);
        }

    }
}