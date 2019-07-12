using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualPropertiesPage : ContentPage
	{
        public ManualPropertiesPageViewModel ViewModel { get; set; }
        public ManualPropertiesPage ()
		{
			InitializeComponent ();
            ViewModel = new ManualPropertiesPageViewModel { Navigation = this.Navigation };
            BindingContext = ViewModel;
		}
	}
}