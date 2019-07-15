using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualTypesObjectsPage : ContentPage
	{
		public ManualTypesObjectsPage()
		{
			InitializeComponent ();
            BindingContext = new ManualTypesObjectsPageViewModel { Navigation = this.Navigation };
		}
	}
}