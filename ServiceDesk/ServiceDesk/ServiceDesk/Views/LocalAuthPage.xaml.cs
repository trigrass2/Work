using ServiceDesk.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocalAuthPage : ContentPage
	{
		public LocalAuthPage ()
		{
			InitializeComponent ();
            BindingContext = new LocalAuthViewModel() { Navigation = this.Navigation };
		}
	}
}