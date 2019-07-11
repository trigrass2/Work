using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualPropertiesPage : ContentPage
	{
		public ManualPropertiesPage ()
		{
			InitializeComponent ();
            BindingContext = new ManualPropertiesPageViewModel { Navigation = this.Navigation};
		}
	}
}