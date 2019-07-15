using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualPage : ContentPage
	{
		public ManualPage ()
		{
			InitializeComponent ();
            BindingContext = new ManualPageViewModel { Navigation = this.Navigation};

        }
	}
}