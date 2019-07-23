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
            //BindingContext = ViewModel;
            
            Content = new CheckListView(ViewModel);
		}

        //private void SfCheckBox_StateChanged(object sender, StateChangedEventArgs e)
        //{
        //    var model = (sender as SfCheckBox).BindingContext as SystemObjectPropertyValueModel;
        //    ViewModel.CreateNewValue(model, e.IsChecked);           
        //}

        //private async void Entry_Completed_float(object sender, System.EventArgs e)
        //{
        //    var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
        //    if(double.TryParse(model.Value as string, out double d) == false)
        //    {
        //        await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.Typename}", "Ошибка", "Ок");
        //        return;
        //    }
        //    ViewModel.CreateNewValue(model, d);
        //}

        //private async void Entry_Completed_int(object sender, System.EventArgs e)
        //{
        //    var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
        //    if (int.TryParse(model.Value as string, out int i) == false)
        //    {
        //        await UserDialogs.Instance.AlertAsync($"Не верный формат данных! Необходимо {model.Typename}", "Ошибка", "Ок");
        //        return;
        //    }
        //    ViewModel.CreateNewValue(model, i);
        //}
        
        //private void Entry_Completed_string(object sender, System.EventArgs e)
        //{
        //    var model = (sender as Entry).BindingContext as SystemObjectPropertyValueModel;
        //    ViewModel.CreateNewValue(model, model.Value);
        //}

    }
}