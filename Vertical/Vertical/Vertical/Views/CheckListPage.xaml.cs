using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.Models;
using Vertical.ViewModels;
using System.Linq;
using Syncfusion.XForms.Buttons;

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

        private void SfCheckBox_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {

           var model = (sender as SfCheckBox).BindingContext as SystemObjectPropertyValueModel;
            ViewModel.CreateNewValue(model, e.IsChecked);
           
        }
    }
}